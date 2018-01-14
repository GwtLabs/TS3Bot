using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using TS3Bot.Core;
using TS3Bot.Core.Libraries;
using TS3Bot.Core.Logging;
using TS3Bot.Core.Model;
using TS3QueryLib.Net.Core.Common.CommandHandling;
using TS3QueryLib.Net.Core.Server.Commands;
using TS3QueryLib.Net.Core.Server.Notification.EventArgs;

namespace TS3Bot.Ext.AutoPoke.Model
{
    class AutoPokeService
    {
        #region Variables

        private AutoPokeExtension extension;
        private ILogger log = Interface.TS3Bot.RootLogger;
        private Server Server = Interface.TS3Bot.GetLibrary<Server>();
        private Lang lang = Interface.TS3Bot.GetLibrary<Lang>();
        private IDictionary<uint, Timer> timers = new Dictionary<uint, Timer>();
        private IDictionary<uint, ChannelData> channels = new Dictionary<uint, ChannelData>();
        private IDictionary<uint, ClientData> clients = new Dictionary<uint, ClientData>();

        private Object TimersLock = new Object();

        private int maxLengthNickname = 20;
        private int delayStart = 5;
        private int staffNotifCooldown = 5;
        private int userNotifCooldown = 10;
        //private int notifLvlToBusyStaff = 2;
        private int maxWaitingTimeWhenStaffIsOnline = 20;

        #endregion Variables

        public AutoPokeService(AutoPokeExtension e)
        {
            extension = e;
        }

        #region Methods

        public void AddChannel(ChannelData channel)
        {
            channels.Add(channel.Id, channel);
        }

        public void ClientMoved(ClientMovedEventArgs e)
        {
            // czy klient był na obserwowanym kanale
            if (WasOnTackedChannel(e.ClientId))
            {
                LeftTrackedChannel(e.ClientId);
            }

            if (channels.ContainsKey(e.TargetChannelId))
            {
                UpdateTimers();

                log.Info($"{DateTime.Now}: Client (clid:{e.ClientId}) moved to channel (cid:{e.TargetChannelId}).");

                ChannelData ch = channels[e.TargetChannelId];

                if (ch.WasStaff)
                {
                    return;
                }

                ClientData cd = GetClientData(e.ClientId);
                if (cd == null)
                {
                    return;
                }
                ch.Join(cd);

                if (ch.NeedHelp)
                {
                    InitializeTimer(ch);
                }
                return;
            }
        }

        private ClientData GetClientData(uint clid)
        {
            if (clients.ContainsKey(clid))
            {
                return clients[clid];
            }
            Client client = Server.GetClient(clid);
            if (client == null)
            {
                return null;
            }
            ClientData cld = new ClientData(client);
            clients.Add(clid, cld);

            return cld;
        }

        #endregion Methods

        #region Helpers

        private bool WasOnTackedChannel(uint clid)
        {
            return channels.Any(c => c.Value.Clients.Any(cl => cl.Object.ClientId == clid));
        }

        private void LeftTrackedChannel(uint clid)
        {
            channels.ForEach(c => c.Value.Left(clid));
            UpdateTimers();
        }

        #endregion Helpers

        #region Timers

        private void UpdateTimers()
        {
            lock (TimersLock)
            {
                foreach (var cd in channels.Values)
                {
                    if (!cd.NeedHelp && timers.ContainsKey(cd.Id))
                    {
                        timers[cd.Id].Stop();
                        timers.Remove(cd.Id);
                        log.Info($"Removed timer for (cid:{cd.Id}).");
                    }
                }
            }
        }

        private void InitializeTimer(ChannelData cd)
        {
            lock (TimersLock)
            {
                if (!timers.ContainsKey(cd.Id))
                {
                    Timer t = new Timer();
                    t.Interval = 1800;
                    t.Enabled = true;
                    t.Elapsed += delegate { ChannelTick(cd); };
                    timers.Add(cd.Id, t);
                    log.Info($"Added timer for (cid:{cd.Id}).");
                }
            }
        }

        private void ChannelTick(ChannelData ch)
        {
            if (ch.WasStaff)
            {
                UpdateTimers();
            }
            List<Client> chStaffOnline = Server.GetClientsWithGroups(ch.GetGroupList());
            string clientName = string.Join(", ", ch.Clients.Select(c => c.Object.Nickname));
            clientName = clientName.Substring(0, Math.Min(maxLengthNickname, clientName.Length));
            Channel channel = Server.GetChannel(ch.Id);

            string clientMsgKey;
            if (chStaffOnline.Count > 0)
            {
                if (ch.LongerTime(delayStart))
                {
                    // Notifications for staff
                    foreach (var s in chStaffOnline)
                    {
                        // TODO: czas reakcji, typ, bądź ignorowanie wiadmości idndywidualne dla każego z obsługi
                        var cd = GetClientData(s.ClientId);
                        if (!cd.HasNotifCooldown(staffNotifCooldown))
                        {
                            log.Info($"Staff Notice (clid:{cd.Object.ClientId}) per (cid:{ch.Id}).");
                            if (ch.WasStaff)
                                return;
                            new ClientPokeCommand(s.ClientId, lang.GetMessage("StaffNotification", extension, s.ClientId)
                                .Replace("{ClientName}", clientName).Replace("{ChannelName}", channel.Name).Replace("{Time}", ch.Time()))
                                .Execute(Interface.TS3Bot.QueryClient);
                            cd.LastNotifNow();
                        }
                    }
                    //ch.NextLevel();
                }
                if (!ch.LongerTime(maxWaitingTimeWhenStaffIsOnline))
                {
                    clientMsgKey = "UserNotification";
                }
                else
                {
                    clientMsgKey = "UserStaffBusyNotification";
                }
            }
            else
            {
                clientMsgKey = "UserNoStaffOnlineNotification";
            }

            // Notifications for clients
            foreach (var cd in ch.Clients)
            {
                if (!cd.HasNotifCooldown(userNotifCooldown))
                {
                    log.Info($"Client Notice '{clientMsgKey}' (clid:{cd.Object.ClientId}) per (cid:{ch.Id}).");
                    if (ch.WasStaff)
                        return;
                    new SendTextMessageCommand(MessageTarget.Client, cd.Object.ClientId, lang.GetMessage(clientMsgKey, extension, cd.Object.ClientId)
                        .Replace("{ClientName}", cd.Object.Nickname)).Execute(Interface.TS3Bot.QueryClient);
                    cd.LastNotifNow();
                }
            }
        }

        #endregion Timers
    }
}
