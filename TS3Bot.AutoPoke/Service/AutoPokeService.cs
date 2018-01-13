using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using TS3QueryLib.Net.Core.Server.Commands;
using TS3QueryLib.Net.Core.Server.Entitities;
using TS3QueryLib.Net.Core.Server.Notification.EventArgs;
using TS3QueryLib.Net.Core.Common.CommandHandling;
using TS3Bot.Core;
using TS3Bot.Core.Libraries;
using System.Linq;
using TS3Bot.Core.Model;
using TS3Bot.Core.Extensions;

namespace TS3Bot.Ext.AutoPoke.Model
{
    class AutoPokeService
    {
        #region Variables

        private Server Server = Interface.TS3Bot.GetLibrary<Server>();
        private Lang lang = Interface.TS3Bot.GetLibrary<Lang>();
        private IDictionary<uint, Timer> timers = new Dictionary<uint, Timer>();
        private IDictionary<uint, ChannelData> channels = new Dictionary<uint, ChannelData>();
        private IDictionary<uint, ClientData> clients = new Dictionary<uint, ClientData>();

        private Object TimersLock = new Object();

        private int maxLengthNickname = 20;

        #endregion Variables

        public AutoPokeService()
        {
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
                Console.WriteLine($"{DateTime.Now}: {e.ClientId} - On target channel {e.TargetChannelId}.");

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
                foreach (var c in channels)
                {
                    if (!c.Value.NeedHelp && timers.ContainsKey(c.Value.Id))
                    {
                        timers[c.Value.Id].Stop();
                        timers.Remove(c.Value.Id);
                    }
                }
            }
        }

        private void InitializeTimer(ChannelData channel)
        {
            lock (TimersLock)
            {
                if (!timers.ContainsKey(channel.Id))
                {
                    Timer t = new Timer();
                    t.Interval = 3000;
                    t.Enabled = true;
                    t.Elapsed += delegate { ChannelTick(channel); };
                    timers.Add(channel.Id, t);
                }
            }
        }

        private void ChannelTick(ChannelData ch)
        {
            List<Client> chStaffOnline = Server.GetClientsWithGroups(ch.GetGroupList());
            string clientName = string.Join(", ", ch.Clients.Select(c => c.Object.Nickname));
            clientName = clientName.Substring(0, Math.Min(maxLengthNickname, clientName.Length));
            Channel channel = Server.GetChannel(ch.Id);

            string clientMsg;
            if (chStaffOnline.Count > 0)
            {
                // Notifications for staff
                foreach (var s in chStaffOnline)
                {
                    var cd = GetClientData(s.ClientId);
                    Console.WriteLine($"Staff check: {s.Nickname}");
                    if (cd.LastNotifAt < DateTime.UtcNow.AddSeconds(-5))
                    {
                        new ClientPokeCommand(s.ClientId, $"[b]{clientName}[/b] czeka na [b]{channel.Name}[/b]").Execute(Interface.TS3Bot.QueryClient);
                        cd.LastNotifAt = DateTime.UtcNow;
                    }
                }
                clientMsg
            }
            else
            {
                clientMsg = "Aktualnie nie ma online nikogo z obsługi kanału";
            }


            // Notifications for clients
            foreach (var c in ch.Clients)
            {
                // "Wait a moment, someone will come to soon."
                Console.WriteLine($"{DateTime.Now}: {c.Object.ClientId} {c.CreatedAt} - Wait a moment, someone will come to soon.");
                c.LastNotifAt = DateTime.UtcNow;
            }
        }

        #endregion Timers
    }
}
