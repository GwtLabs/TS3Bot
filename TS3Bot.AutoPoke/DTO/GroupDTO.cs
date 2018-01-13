namespace TS3Bot.Ext.AutoPoke.DTO
{
    public class GroupDTO
    {
        public uint Id { get; set; }
        // wyłącza powiadomienia z kanału nawet jeśli nie otrzymuje powiadomień z niego
        public bool SuperGroup { get; set; } = false;
        public int DelayRelative { get; set; } = 0;
        public int DelayAbsolute { get; set; } = 0;
    }
}
