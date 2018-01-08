namespace TS3Bot.Core.Config
{
    public class Ts3BotConfig
    {
        public Ts3BotConfig()
        {
            Server = new Server();
            Bot = new Bot();
        }
        public Server Server { get; set; }
        public Bot Bot { get; set; }
    }
}
