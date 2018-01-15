using System;
using TS3Bot.Core.Model;

namespace TS3Bot.Ext.AutoPoke.Model
{
    public class ClientData
    {
        public Client Object { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime LastNotifAt { get; private set; }

        public ClientData(Client client)
        {
            Object = client;
            CreatedAt = DateTime.UtcNow;
        }

        public void LastNotifNow()
        {
            LastNotifAt = DateTime.UtcNow;
        }

        public bool HasNotifCooldown(int seconds)
        {
            return LastNotifAt > DateTime.UtcNow.AddSeconds(-seconds);
        }
    }
}
