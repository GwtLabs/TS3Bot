using System;
using System.Collections.Generic;
using System.Text;

namespace TS3Bot.Ext.AutoPoke.DTO
{
    public class ConfigDTO
    {
        public bool Enabled { get; set; } = true;
        public Dictionary<uint, List<ChannelDTO>> Channels = new Dictionary<uint, List<ChannelDTO>>();

        public ConfigDTO(bool Initialization = false)
        {
            if (Initialization)
                Channels.Add(2, new List<ChannelDTO>() { new ChannelDTO(Initialization) });
        }
    }
}
