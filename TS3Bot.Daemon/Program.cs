using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TS3QueryLib.Net.Core;
using TS3QueryLib.Net.Core.Common;
using TS3QueryLib.Net.Core.Common.Responses;
using TS3QueryLib.Net.Core.Server.Commands;
using TS3QueryLib.Net.Core.Server.Entitities;
using TS3QueryLib.Net.Core.Server.Notification;
using ea = TS3QueryLib.Net.Core.Server.Notification.EventArgs;
using TS3QueryLib.Net.Core.Server.Responses;
using TS3Bot.Core;
using TS3Bot.Ext.AutoPoke;

namespace TS3Bot.Daemon
{
    class Program
    {
        static void Main(string[] args)
        {
            var TS3bot = TS3BotCore.Instance;

            TS3bot.AddExtension(new AutoPokeExtension());

            TS3bot.Run();
        }
    }
}
