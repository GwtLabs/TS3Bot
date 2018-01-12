using TS3Bot.Core.Extensions;
using TS3Bot.Ext.AutoPoke.DTO;
using TS3QueryLib.Net.Core.Server.Notification;
using TS3QueryLib.Net.Core.Server.Notification.EventArgs;
using System;
using System.Collections.Generic;
using TS3Bot.Ext.AutoPoke.Model;
using TS3Bot.Ext.AutoPoke.Mappers;
using AutoMapper;

namespace TS3Bot.Ext.AutoPoke
{
    public class AutoPokeExtension : Extension
    {
        public override string Name => "AutoPoke";

        private static ConfigDTO config;
        private static IMapper mapper;
        private static AutoPokeService channelService;

        private static IDictionary<uint, bool> events;
        private static Object eventLock = new Object();

        public AutoPokeExtension()
        {
            config = GetConfig<ConfigDTO>();
            if (config.Enabled)
            {
                mapper = AutoMapperConfig.Initialize();

                events = new Dictionary<uint, bool>();
                channelService = new AutoPokeService();

                foreach (var c in config.Channels)
                {
                    channelService.AddChannel(mapper.Map<ChannelDTO, ChannelData>(c));
                }
            }
        }

        protected override void LoadDefaultConfig()
        {
            SetConfig(new ConfigDTO()
            {
                Enabled = true,
                Channels = new List<ChannelDTO>() {
                    new ChannelDTO() {
                        Id = 2,
                        StaffGroups = new List<GroupDTO>() {
                            { new GroupDTO() {Id=234, DelayAbsolute=0, DelayRelative=0} },
                            { new GroupDTO() {Id=235, DelayAbsolute=0, DelayRelative=0} }
                        }
                    }
                }
            });
        }

        public override void RegisterNotifications(NotificationHub notifications)
        {
            notifications.ClientMoved.JoiningChannel += ClientMoved_JoiningChannel;
            notifications.ClientMoved.JoiningChannelForced += ClientMoved_JoiningChannelForced;
        }

        private static void ClientMoved_JoiningChannelForced(object sender, ClientMovedByClientEventArgs e)
        {
            ClientJoinToChannel(sender, e);
        }

        private static void ClientMoved_JoiningChannel(object sender, ClientMovedEventArgs e)
        {
            ClientJoinToChannel(sender, e);
        }

        private static void ClientJoinToChannel(object sender, ClientMovedEventArgs e)
        {
            lock (eventLock)
            {
                if (events.ContainsKey(e.ClientId))
                {
                    events.Remove(e.ClientId);
                    return;
                }
                events.Add(e.ClientId, true);

                channelService.ClientMoved(e);
            }
        }
    }
}
