using System;
using System.Collections.Generic;
using System.Text;

namespace TS3Bot.Ext.AutoPoke.DTO
{
    public class GroupDTO
    {
        public ushort Id { get; set; }
        public int DelayRelative { get; set; } = 0;
        public int DelayAbsolute { get; set; } = 0;
    }
}
