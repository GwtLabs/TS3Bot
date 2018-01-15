using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TS3Bot.Core.Mappers;
using TS3Bot.Core.Model;
using TS3QueryLib.Net.Core;
using TS3QueryLib.Net.Core.Server.Commands;
using TS3QueryLib.Net.Core.Server.Entitities;
using TS3QueryLib.Net.Core.Server.Notification;
using TS3QueryLib.Net.Core.Server.Notification.EventArgs;

namespace TS3Bot.Core.Libraries
{
    public class Server : Library
    {
        #region Variables

        private static Dictionary<uint, Channel> channels = new Dictionary<uint, Channel>();
        private static Dictionary<uint, Client> clients = new Dictionary<uint, Client>();

        private static IDictionary<uint, DateTime> events;
        private static Object eventLock = new Object();
        private static Object channelsLock = new Object();
        private static Object clientsLock = new Object();
        private static Dictionary<uint, Object> channelLock = new Dictionary<uint, Object>();
        private static Dictionary<uint, Object> clientLock = new Dictionary<uint, Object>();

        private static IMapper mapper;

        public IQueryClient client;

        #endregion Variables

        #region Initialization

        public Server()
        {
            events = new Dictionary<uint, DateTime>();
            mapper = AutoMapperConfig.Initialize();
        }

        #endregion Initialization

        #region Notifications

        public override void RegisterNotifications(NotificationHub notifications)
        {
            notifications.ClientJoined.Triggered += ClientJoined_Triggered;
            notifications.ClientMoved.JoiningChannel += ClientMoved_JoiningChannel;
            notifications.ClientMoved.CreatingTemporaryChannel += ClientMoved_CreatingTemporaryChannel;
            notifications.ClientMoved.JoiningChannelForced += ClientMoved_JoiningChannelForced;
            notifications.ClientLeft.Kicked += ClientLeft_Kicked;
            notifications.ClientLeft.Disconnected += ClientLeft_Disconnected;
            notifications.ClientLeft.ConnectionLost += ClientLeft_ConnectionLost;
            notifications.ClientLeft.Banned += ClientLeft_Banned;
        }

        private static void ClientJoined_Triggered(object sender, ClientJoinedEventArgs e)
        {
            ClientJoined(e);
        }

        private static void ClientMoved_JoiningChannelForced(object sender, ClientMovedByClientEventArgs e)
        {
            ClientMoved(sender, e);
        }

        private static void ClientMoved_CreatingTemporaryChannel(object sender, ClientMovedEventArgs e)
        {
            ClientMoved(sender, e);
        }

        private static void ClientMoved_JoiningChannel(object sender, ClientMovedEventArgs e)
        {
            ClientMoved(sender, e);
        }

        private static void ClientLeft_Banned(object sender, ClientBanEventArgs e)
        {
            ClientLeft(e.VictimClientId);
        }

        private static void ClientLeft_ConnectionLost(object sender, ClientConnectionLostEventArgs e)
        {
            ClientLeft(e.ClientId);
        }

        private static void ClientLeft_Disconnected(object sender, ClientDisconnectEventArgs e)
        {
            ClientLeft(e.ClientId);
        }

        private static void ClientLeft_Kicked(object sender, ClientKickEventArgs e)
        {
            ClientLeft(e.VictimClientId);
        }

        #endregion Notifications

        #region API

        public static Client GetClient(uint clid)
        {
            return clients.ContainsKey(clid) ? clients[clid] : null;
        }

        public static Dictionary<uint, Client> GetClients()
        {
            return clients;
        }

        public static List<Client> GetClientsWithGroup(uint gid)
        {
            return clients.Where(c => c.Value.ServerGroups.Contains(gid)).Select(c => c.Value).ToList();
        }

        public static List<Client> GetClientsWithGroups(List<uint> gids)
        {
            //return clients.Where(c => GinG(c.Value.ServerGroups, gids)).Select(c => c.Value).ToList();
            return clients.Where(c => c.Value.ServerGroups.Intersect(gids).Any()).Select(c => c.Value).ToList();
        }

        //private static bool GinG(IList<uint> list1, IList<uint> list2)
        //{
        //    return list1.Intersect(list2).Any();
        //}

        public static Channel GetChannel(uint cid)
        {
            return channels.ContainsKey(cid) ? channels[cid] : null;
        }

        #endregion API

        #region Methods

        public void UpdateServerData()
        {
            UpdateChannels();
            UpdateClients();
        }

        private void UpdateChannels()
        {
            lock (channelsLock)
            {
                var response = new ChannelListCommand(includeAll: false, includeTopics: true).Execute(Interface.TS3Bot.QueryClient);
                if (response.IsErroneous)
                {
                    return;
                }

                foreach (var c in response.Values)
                {
                    UpdateChannel(mapper.Map<ChannelListEntry, Channel>(c));
                }
            }
        }

        private void UpdateClients()
        {
            lock (clientsLock)
            {
                var response = new ClientListCommand(includeUniqueId: true, includeGroupInfo: true, includeCountry: true).Execute(Interface.TS3Bot.QueryClient);
                if (response.IsErroneous)
                {
                    return;
                }

                foreach (var c in response.Values)
                {
                    UpdateClient(mapper.Map<ClientListEntry, Client>(c));
                }
            }
        }

        private void UpdateChannel(Channel channel)
        {
            lock (ChannelLock(channel.ChannelId))
            {
                if (channels.ContainsKey(channel.ChannelId))
                {
                    channels[channel.ChannelId] = channel;
                }
                else
                {
                    channels.Add(channel.ChannelId, channel);
                }
            }
        }

        private void UpdateClient(Client client)
        {
            lock (ClientLock(client.ClientId))
            {
                if (clients.ContainsKey(client.ClientId))
                {
                    clients[client.ClientId] = client;
                }
                else
                {
                    clients.Add(client.ClientId, client);
                }
            }
        }

        private static void ClientJoined(ClientJoinedEventArgs e)
        {
            if (DoubleEvent(e.ClientId))
                return;

            lock (ClientLock(e.ClientId))
            {
                if (!clients.ContainsKey(e.ClientId))
                {
                    clients.Add(e.ClientId, mapper.Map<ClientJoinedEventArgs, Client>(e));
                }
                else
                {
                    clients[e.ClientId] = mapper.Map<ClientJoinedEventArgs, Client>(e);
                }
            }
        }

        private static void ClientMoved(object sender, ClientMovedEventArgs e)
        {
            if (DoubleEvent(e.ClientId))
                return;

            lock (ClientLock(e.ClientId))
            {
                if (clients.ContainsKey(e.ClientId))
                {
                    // TODO: pobrać więcej danych, albo oznaczyć, że wymaga aktualizacji grup kanałowych itp..
                    clients[e.ClientId].MovedToChannel(e.TargetChannelId);
                }
            }
        }

        private static void ClientLeft(uint clid)
        {
            if (DoubleEvent(clid))
                return;

            lock (ClientLock(clid))
            {
                if (clients.ContainsKey(clid))
                {
                    clients.Remove(clid);
                }
            }
        }

        #endregion Methods

        #region Helpers

        private static bool DoubleEvent(uint id)
        {
            lock (eventLock)
            {
                if (events.ContainsKey(id))
                {
                    events.Remove(id);

                    return true;
                }
                // TODO: Dodać usuwanie starych eventów po dacie dodania co np kilka godzin? (oficjalnie events powinno się czyścić same zdublowanym eventem)
                events.Add(id, DateTime.UtcNow);

                return false;
            }
        }

        private static Object ChannelLock(uint id)
        {
            if (!channelLock.ContainsKey(id))
            {
                channelLock.Add(id, new Object());
            }
            return channelLock[id];
        }

        private static Object ClientLock(uint id)
        {
            if (!clientLock.ContainsKey(id))
            {
                clientLock.Add(id, new Object());
            }
            return clientLock[id];
        }

        #endregion Helpers
    }
}
