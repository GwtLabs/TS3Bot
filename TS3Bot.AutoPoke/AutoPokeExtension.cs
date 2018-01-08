using System;
using TS3QueryLib.Net.Core.Server.Notification;
using TS3QueryLib.Net.Core.Server.Notification.EventArgs;
using System.Collections.Generic;
using TS3Bot.Ext.AutoPoke.DTO;
using TS3Bot.Core.Extensions;

namespace TS3Bot.Ext.AutoPoke
{
    public class AutoPokeExtension : Extension
    {
        private static ConfigDTO config;

        public AutoPokeExtension()
        {
            config = new ConfigDTO(true);

            // TODO: ładować z pliku
            config.Channels.Add(0, new List<ChannelDTO>() { new ChannelDTO(true) });
            config.Channels.Add(1, new List<ChannelDTO>() { new ChannelDTO(true) });

            config.Channels.Add(3, new List<ChannelDTO>() { new ChannelDTO(true) });
            config.Channels.Add(4, new List<ChannelDTO>() { new ChannelDTO(true) });
            config.Channels.Add(5, new List<ChannelDTO>() { new ChannelDTO(true) });
            config.Channels.Add(6, new List<ChannelDTO>() { new ChannelDTO(true) });
            config.Channels.Add(7, new List<ChannelDTO>() { new ChannelDTO(true) });
        }

        public override void RegisterNotifications(NotificationHub notifications)
        {
            notifications.ClientMoved.JoiningChannel += ClientMoved_JoiningChannel;
            notifications.ClientMoved.JoiningChannelForced += ClientMoved_JoiningChannelForced;
        }


        private static void ClientMoved_JoiningChannelForced(object sender, ClientMovedByClientEventArgs e)
        {
            Console.WriteLine($"AutoPoke Move: Type=Forced, ClientId={e.ClientId}, TargetChannelId={e.TargetChannelId}, Invoker={e.InvokerNickname}");
            ClientJoinToChannel(sender, e);
        }


        private static void ClientMoved_JoiningChannel(object sender, ClientMovedEventArgs e)
        {
            Console.WriteLine($"AutoPoke Move: Type=Self, ClientId={e.ClientId}, TargetChannelId={e.TargetChannelId}");
            ClientJoinToChannel(sender, e);
        }

        private static void ClientJoinToChannel(object sender, ClientMovedEventArgs e)
        {
            if (config.Channels.ContainsKey(e.TargetChannelId))
            {
                Console.WriteLine($"AutoPoke!!!! cid:{e.TargetChannelId}");
            }
        }
    }
}
