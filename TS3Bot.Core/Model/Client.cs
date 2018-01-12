using TS3QueryLib.Net.Core.Server.Entitities;

namespace TS3Bot.Core.Model
{
    public class Client : ClientListEntry
    {
        public bool NeedUpdate { get; private set; } = false;
        public uint PreviousChannelId { get; set; }

        public Client()
        {

        }

        public void MovedToChannel(uint cid)
        {
            NeedUpdate = true;
            PreviousChannelId = ChannelId;
            ChannelId = cid;
        }
    }
}
