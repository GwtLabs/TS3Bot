namespace TS3Bot.Ext.AutoPoke.Model
{
    public class ClientData
    {
        public bool IsStaff { get; } = false;
        public uint Id { get; }
        public ChannelData Channel { get; set; }

        public ClientData(uint id, bool isStaff = false)
        {
            Id = id;
            IsStaff = isStaff;
        }
    }
}
