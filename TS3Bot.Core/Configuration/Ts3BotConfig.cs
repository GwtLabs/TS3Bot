namespace TS3Bot.Core.Configuration
{
    public class Ts3BotConfig
    {
        public ServerOptions Server { get; set; }
        public BotOptions Bot { get; set; }

        public class ServerOptions
        {
            public string Host { get; set; }
            public ushort Port { get; set; }
            public ServerQueryOptions Query { get; set; }
        }

        public class ServerQueryOptions
        {
            public ushort Port { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
        }

        public class BotOptions
        {
            public string Name { get; set; }
            public ushort DefaultChannelId { get; set; }
        }

        public Ts3BotConfig()
        {
            Server = new ServerOptions() { Host = "localhost", Port = 9987, Query = new ServerQueryOptions() { Port = 10011, Login = "serveradmin", Password = "" } };
            Bot = new BotOptions() { Name = "[Bot] AutoPoke", DefaultChannelId = 1 };
        }
    }
}
