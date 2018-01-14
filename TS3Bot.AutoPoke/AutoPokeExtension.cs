﻿using AutoMapper;
using System;
using System.Collections.Generic;
using TS3Bot.Core.Extensions;
using TS3Bot.Ext.AutoPoke.DTO;
using TS3Bot.Ext.AutoPoke.Mappers;
using TS3Bot.Ext.AutoPoke.Model;
using TS3QueryLib.Net.Core.Server.Notification;
using TS3QueryLib.Net.Core.Server.Notification.EventArgs;

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
                channelService = new AutoPokeService(this);

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

        #region Localization

        protected override void LoadDefaultMessages()
        {
            // English
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["UserNotification"] = "[b][color=green]Dear {ClientName}[/color]. [color=red]Wait a moment, someone will come to soon.[/color][/b]",
                ["UserStaffBusyNotification"] = "[b]Wszyscy admini wydają się być aktualnie czymś zajęci i nie mogą w tej chwili pomóc. Możesz poczekąć (zapewne dłużej niż zwykle), albo spróbować ponownie trochę później.[/b]",
                ["UserNoStaffOnlineNotification"] = "[b]Przepraszamy, ale w tej chwili nie ma ani jednego admina online. Spróbuj ponownie później.[/b]",
                ["StaffNotification"] = "[u]#{Time}[/u] [b]{ClientName}[/b] czeka na [b]{ChannelName}[/b]",
            }, this);

            // Polish
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["UserNotification"] = "[b][color=green]Drogi {ClientName}[/color]. [color=red]Administracja została powiadomiona o Twoim pobycie na tym kanale. Za chwilę ktoś Ci pomoże.[/color][/b]",
                ["UserStaffBusyNotification"] = "[b]Wszyscy admini wydają się być aktualnie czymś zajęci i nie mogą w tej chwili pomóc. Możesz poczekąć (zapewne dłużej niż zwykle), albo spróbować ponownie trochę później.[/b]",
                ["UserNoStaffOnlineNotification"] = "[b]Przepraszamy, ale w tej chwili nie ma ani jednego admina online. Spróbuj ponownie później.[/b]",
                ["StaffNotification"] = "[u]#{Time}[/u] [b]{ClientName}[/b] czeka na [b]{ChannelName}[/b]",
            }, this, "pl");
        }

        #endregion Localization


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
