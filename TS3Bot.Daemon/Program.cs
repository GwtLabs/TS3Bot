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
