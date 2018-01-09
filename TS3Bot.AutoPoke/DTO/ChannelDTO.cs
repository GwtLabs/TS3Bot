using System;
using System.Collections.Generic;
using System.Text;

namespace TS3Bot.Ext.AutoPoke.DTO
{
    public class ChannelDTO
    {
        public uint Id { get; set; }
        public List<GroupDTO> Groups = new List<GroupDTO>();

        //public ChannelDTO(bool Initialization = false)
        //{
        //    if (Initialization)
        //        Groups = new List<GroupDTO>() { new GroupDTO() };
        //}
    }
}
