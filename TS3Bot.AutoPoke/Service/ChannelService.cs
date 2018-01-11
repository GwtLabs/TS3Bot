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

namespace TS3Bot.Ext.AutoPoke.Model
{
    class ChannelService
    {
        private Server Server = Interface.TS3Bot.GetLibrary<Server>();
        private IDictionary<uint, Timer> timers = new Dictionary<uint, Timer>();
        private IDictionary<uint, ChannelData> channels = new Dictionary<uint, ChannelData>();
        private IDictionary<uint, ClientData> clients = new Dictionary<uint, ClientData>();

        public ChannelService()
        {
        }

        public void AddChannel(ChannelData channel)
        {
            channels.Add(channel.Id, channel);
        }

        public void ClientMoved(ClientMovedEventArgs e)
        {
            // czy klient był na obserwowanym kanale
            if (WasOnTackedChannel(e.ClientId))
            {
                //LeftTrackedChannel(e.ClientId);
            }

            if (channels.ContainsKey(e.TargetChannelId))
            {
                Console.WriteLine($"{DateTime.Now}: {e.ClientId} - On target channel {e.TargetChannelId}.");

                ChannelData ch = channels[e.TargetChannelId];

                if (ch.WasStaff)
                {
                    return;
                }

                Client client = Server.GetClient(e.ClientId);
                if (client != null)
                    ch.Join(client);

                if (ch.NeedHelp)
                {
                    InitializeTimer(ch);
                }
                return;
            }
        }

        private bool WasOnTackedChannel(uint clid)
        {
            return channels.Any(c => c.Value.Clients.Any(cl => cl.Id == clid));
        }

        private void LeftTrackedChannel()
        {
            //channels.Rem
            //channels.Where(c => c.Value.)
        }

        private void InitializeTimer(ChannelData channel)
        {
            if (!timers.ContainsKey(channel.Id))
            {
                Timer t = new Timer();
                t.Interval = 10000;
                t.Enabled = true;
                t.Elapsed += delegate { ChannelTick(channel); };
                //try
                //{
                timers.Add(channel.Id, t);
                //}
                //catch (Exception e)
                //{
                //    new Exception();
                //}
            }
        }

        private void ChannelTick(ChannelData ch)
        {
            //Server.GetClient(87);
            foreach (var c in ch.Clients)
            {
                //new SendTextMessageCommand(MessageTarget.Client, c.Id, "Wait a moment, someone will come to soon.");
                Console.WriteLine($"{DateTime.Now}: {c.Id} - Wait a moment, someone will come to soon.");
            }
        }
    }
}
