using System;
using System.Collections.Generic;
using System.Text;
using TS3QueryLib.Net.Core.Server.Notification;

namespace TS3Bot.Core.Extensions
{
    public abstract class Extension
    {
        public abstract void RegisterNotifications(NotificationHub notifications);
    }
}
