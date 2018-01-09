using System;
using System.Collections.Generic;
using System.Text;
using TS3QueryLib.Net.Core.Server.Entitities;

namespace TS3Bot.Ext.AutoPoke.Model
{
    class Channel
    {
        public uint Id { get; }
        public bool WasStaff { get; }
        public List<Client> Clients { get; } = new List<Client>();

        public Channel(uint id)
        {
            Id = id;
        }

        public void Join(ClientListEntry client)
        {
            Clients.Add(new Client(id: client.ClientId));
        }
    }
}
