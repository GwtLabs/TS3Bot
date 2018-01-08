namespace TS3Bot.Core.Config
{
    public class Server
    {
        public const ushort DefaultPort = 9987;
        public const ushort DefaultQueryPort = 10011;

        public string Host { get; set; } = "localhost";
        public ushort Port { get; set; } = Server.DefaultPort;
        public ServerQuery Query { get; set; } = new ServerQuery();

        public class ServerQuery
        {
            public ushort Port { get; set; } = Server.DefaultQueryPort;
            public string Login { get; set; } = "serveradmin";
            public string Password { get; set; } = "";
        }
    }
}
