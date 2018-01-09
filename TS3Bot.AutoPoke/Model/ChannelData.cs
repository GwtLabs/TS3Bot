using System;
using System.Collections.Generic;
using System.Text;
using TS3QueryLib.Net.Core.Server.Entitities;

namespace TS3Bot.Ext.AutoPoke.Model
{
    class ChannelData
    {
        public uint Id { get; }
        public bool WasStaff { get; }
        public List<ClientData> Clients { get; } = new List<ClientData>();

        public ChannelData(uint id)
        {
            Id = id;
        }

        public void Join(ClientListEntry client)
        {
            Clients.Add(new ClientData(id: client.ClientId));
        }
    }
}
