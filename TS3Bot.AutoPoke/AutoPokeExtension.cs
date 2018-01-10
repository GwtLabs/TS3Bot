using TS3Bot.Core.Extensions;
using TS3Bot.Ext.AutoPoke.DTO;
using TS3QueryLib.Net.Core.Server.Notification;
using TS3QueryLib.Net.Core.Server.Notification.EventArgs;
using System;
using System.Collections.Generic;
using TS3Bot.Ext.AutoPoke.Model;
using TS3QueryLib.Net.Core.Server.Entitities;
using TS3Bot.Ext.AutoPoke.Mappers;
using AutoMapper;

namespace TS3Bot.Ext.AutoPoke
{
    public class AutoPokeExtension : Extension
    {
        public override string Name => "AutoPoke";

        private static ConfigDTO config;
        private static IMapper mapper;
        private static ChannelService channelService;

        public AutoPokeExtension()
        {
            config = GetConfig<ConfigDTO>();
            if (config.Enabled)
            {
                mapper = AutoMapperConfig.Initialize();
                channelService = new ChannelService(Server);

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
            channelService.Join(e);
        }
    }
}
