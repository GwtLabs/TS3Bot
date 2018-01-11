using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using TS3Bot.Core.Model;
using TS3Bot.Ext.AutoPoke.DTO;
using TS3QueryLib.Net.Core.Server.Commands;
using TS3QueryLib.Net.Core.Server.Entitities;
using TS3QueryLib.Net.Core.Server.Notification.EventArgs;
using TS3QueryLib.Net.Core.Common.CommandHandling;
using TS3Bot.Core;
using TS3Bot.Core.Libraries;

namespace TS3Bot.Ext.AutoPoke.Model
{
    class ChannelService
    {
        private Dictionary<uint, Timer> timers = new Dictionary<uint, Timer>();
        private static Dictionary<uint, ChannelData> channels = new Dictionary<uint, ChannelData>();
        private Server Server = Interface.TS3Bot.GetLibrary<Server>();

        public ChannelService()
        {
        }

        public void AddChannel(ChannelData channel)
        {
            channels.Add(channel.Id, channel);
        }

        public void ClientMoved(ClientMovedEventArgs e)
        {
            if (channels.ContainsKey(e.TargetChannelId))
            {
                ChannelData ch = channels[e.TargetChannelId];

                if (ch.WasStaff)
                {
                    return;
                }

                ClientListEntry client = Server.GetClient(e.ClientId);

                ch.Join(client);

                if (ch.NeedHelp)
                {
                    InitializeTimer(ch);
                }
                return;
            }
        }

        private void InitializeTimer(ChannelData channel)
        {
            if (!timers.ContainsKey(channel.Id))
            {
                Timer t = new Timer();
                t.Interval = 1000;
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
                new SendTextMessageCommand(MessageTarget.Client, c.Id, "Wait a moment, someone will come to soon.");
            }
        }
    }
}
