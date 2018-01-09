using System;
using System.Collections.Generic;
using System.Text;
using TS3Bot.Core.Extensions;

namespace TS3Bot.Ext.AutoPoke.DTO
{
    public class ConfigDTO : IConfig
    {
        public bool Enabled { get; set; } = true;
        public List<ChannelDTO> Channels = new List<ChannelDTO>();
    }
}
