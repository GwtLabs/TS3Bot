using TS3Bot.Core.Model;

namespace TS3Bot.Ext.AutoPoke.Model
{
    public class ClientData
    {
        public bool IsStaff { get; set; } = false;
        public uint Id { get; }
        public ChannelData Channel { get; set; }
        public Client Client { get; set; }

        //public ClientData(uint id, bool isStaff = false)
        //{
        //    Id = id;
        //    IsStaff = isStaff;
        //}
    }
}
