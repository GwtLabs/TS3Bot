using System;
using TS3Bot.Core.Model;

namespace TS3Bot.Ext.AutoPoke.Model
{
    public class ClientData
    {
        public Client Object { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime LastNotifAt { get; set; }
        //private Client _client;
        //public Client Client
        //{
        //    get
        //    {
        //        return _client;
        //    }
        //    set
        //    {
        //        Id = value.ClientId;
        //        _client = value;
        //    }
        //}

        public ClientData(Client client)
        {
            Object = client;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
