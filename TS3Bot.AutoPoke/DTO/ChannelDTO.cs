using System.Collections.Generic;

namespace TS3Bot.Ext.AutoPoke.DTO
{
    public class ChannelDTO
    {
        public uint Id { get; set; }
        public List<GroupDTO> StaffGroups;
    }
}
