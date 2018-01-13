using TS3Bot.Core.Model;

namespace TS3Bot.Ext.AutoPoke.Model
{
    public class ClientData
    {
        public bool IsStaff { get; set; } = false;
        public uint Id { get; private set; }
        public ChannelData Channel { get; set; }
        private Client _client;
        public Client Client
        {
            get
            {
                return _client;
            }
            set
            {
                Id = value.ClientId;
                _client = value;
            }
        }

        //public ClientData(uint id, bool isStaff = false)
        //{
        //    Id = id;
        //    IsStaff = isStaff;
        //}
    }
}
