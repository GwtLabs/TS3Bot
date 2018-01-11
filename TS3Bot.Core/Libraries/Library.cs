using System;
using System.Collections.Generic;
using System.Text;
using TS3QueryLib.Net.Core.Server.Notification;

namespace TS3Bot.Core.Libraries
{
    public abstract class Library
    {
        public abstract void RegisterNotifications(NotificationHub notifications);
    }
}
