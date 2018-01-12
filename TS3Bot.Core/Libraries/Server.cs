using AutoMapper;
using System;
using System.Collections.Generic;
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
        private static Dictionary<uint, Client> clients = new Dictionary<uint, Client>();

        private static IDictionary<uint, DateTime> events;
        private static Object eventLock = new Object();
        private static Object clientsLock = new Object();
        private static Object clientLock = new Object();

        private static IMapper mapper;

        public IQueryClient client;

        public Server()
        {
            events = new Dictionary<uint, DateTime>();
            mapper = AutoMapperConfig.Initialize();
        }

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

        public void UpdateServerData()
        {
            UpdateClients();
        }

        private void UpdateClients()
        {
            lock (clientsLock)
            {
                var response = new ClientListCommand(includeUniqueId: true).Execute(Interface.TS3Bot.QueryClient);
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

        private void UpdateClient(Client client)
        {
            lock (clientLock)
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

        public static Client GetClient(uint clid)
        {
            return clients.ContainsKey(clid) ? clients[clid] : null;
        }

        #region Notifications

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

        #region Methods

        private static void ClientJoined(ClientJoinedEventArgs e)
        {
            lock (eventLock)
            {
                if (DoubleEvent(e.ClientId))
                    return;

                if (!clients.ContainsKey(e.ClientId))
                {
                    clients.Add(e.ClientId, mapper.Map<ClientJoinedEventArgs, Client>(e));
                }
            }
        }

        private static void ClientMoved(object sender, ClientMovedEventArgs e)
        {
            lock (eventLock)
            {
                if (DoubleEvent(e.ClientId))
                    return;

                if (clients.ContainsKey(e.ClientId))
                {
                    // TODO: pobrać więcej danych, albo oznaczyć, że wymaga aktualizacji grup kanałowych itp..
                    clients[e.ClientId].MovedToChannel(e.TargetChannelId);
                }
            }
        }

        private static void ClientLeft(uint clid)
        {
            lock (eventLock)
            {
                if (DoubleEvent(clid))
                    return;

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
            if (events.ContainsKey(id))
            {
                events.Remove(id);
                return true;
            }
            // TODO: Dodać usuwanie starych eventów po dacie dodania co np kilka godzin? (oficjalnie events powinno się czyścić same zdublowanym eventem)
            events.Add(id, DateTime.UtcNow);
            return false;
        }

        #endregion Helpers
    }
}
