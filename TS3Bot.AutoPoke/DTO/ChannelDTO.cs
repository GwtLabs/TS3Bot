using System;
using System.Collections.Generic;
using System.Text;

namespace TS3Bot.Ext.AutoPoke.DTO
{
    public class ChannelDTO
    {
        public uint Id { get; set; }
        public List<GroupDTO> StaffGroups = new List<GroupDTO>();
    }
}
