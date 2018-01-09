using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TS3QueryLib.Net.Core;
using TS3QueryLib.Net.Core.Server.Commands;
using TS3QueryLib.Net.Core.Server.Entitities;

namespace TS3Bot.Core.Services
{
    public class ServerService
    {
        //private Dictionary<uint, Client> clients = new Dictionary<uint, Client>();
        private List<ClientListEntry> clients = new List<ClientListEntry>();

        public IQueryClient client;

        public ServerService(IQueryClient queryClient)
        {
            client = queryClient;
        }

        public void UpdateServerData()
        {
            var response = new ClientListCommand(includeUniqueId: true).Execute(client);
            if (response.IsErroneous)
            {
                return;
            }
            clients = (List<ClientListEntry>)response.Values;
        }

        public ClientListEntry GetClient(uint clid)
        {
            return clients.Where(c => c.ClientId == clid).First();
        }
    }
}
