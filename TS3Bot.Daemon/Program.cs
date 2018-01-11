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
            Interface.Initialize();
            Interface.TS3Bot.AddExtension(new AutoPokeExtension());
            Interface.TS3Bot.Run();
        }
    }
}
