using System.Collections.Generic;
using TS3Bot.Core.Extensions;

namespace TS3Bot.Ext.AutoPoke.DTO
{
    public class ConfigDTO : IConfig
    {
        public bool Enabled { get; set; } = true;
        public List<ChannelDTO> Channels = new List<ChannelDTO>();
        public int MaxLengthNicknames { get; set; } = 30;
        public int DelayNotifStart { get; set; } = 5;
        public int StaffNotifCooldown { get; set; } = 20;
        public int UserNotifCooldown { get; set; } = 40;
        public int MaxWaitingTimeWhenStaffIsOnline { get; set; } = 600;
    }
}
