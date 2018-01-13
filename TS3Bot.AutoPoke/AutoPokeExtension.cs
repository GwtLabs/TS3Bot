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

        #region Localization

        protected override void LoadDefaultMessages()
        {
            // English
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["Added"] = "Player '{name}' has been added to ignored list",
                ["AlreadyExists"] = "Player '{name}' is already on the ignored list",
                ["Cleaned"] = "List has been cleared",
                ["IncorectValue"] = "Invalid value",
                ["List"] = "Ignored list:",
                ["ListIsEmpty"] = "The ignored list is empty",
                ["MultiplePlayersFound"] = "Multiple matching players found: {players}",
                ["NotAddYourself"] = "You can not add yourself",
                ["PlayerNotFound"] = "Could not find player with name '{search}'",
                ["Syntax"] = "Proper {cmd} command usage:",
                ["SyntaxList"] = "Ignored list: {cmd}",
                ["SyntaxAdd"] = "Add to list: {cmd}",
                ["SyntaxRemove"] = "Remove from the list: {cmd}",
                ["SyntaxRemoveId"] = "Remove the id from the list: {cmd}",
                ["SyntaxClear"] = "Clearing the list: {cmd}",
                ["Removed"] = "Removed player '{name}' from ignored list",
            }, this);

            // Polish
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["Added"] = "Gracz '{name}' został dodany do listy ignorowanych",
                ["AlreadyExists"] = "Gracz '{name}' jest już na liście ignorowanych",
                ["Cleaned"] = "Lista została wyczyszczona",
                ["IncorectValue"] = "Nieprawidłowa wartość",
                ["List"] = "Lista ignorowanych:",
                ["ListIsEmpty"] = "Lista ignorowanych jest pusta",
                ["MultiplePlayersFound"] = "Znaleziono kilku pasujących graczy: {players}",
                ["NotAddYourself"] = "Nie możesz dodać siebie samego",
                ["PlayerNotFound"] = "Nie znaleziono gracza '{search}'",
                ["Syntax"] = "Prawidłowe używanie komedny {cmd}:",
                ["SyntaxList"] = "Lista ignorowanych: {cmd}",
                ["SyntaxAdd"] = "Dodanie do listy: {cmd}",
                ["SyntaxRemove"] = "Usunięcie z listy: {cmd}",
                ["SyntaxRemoveId"] = "Usunięcie po id z listy: {cmd}",
                ["SyntaxClear"] = "Wyczyszczenie listy: {cmd}",
                ["Removed"] = "Usunięto z listy ignorowanych gracza '{name}'",
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
