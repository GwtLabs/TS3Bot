using System.Collections.Generic;
using TS3Bot.Core.Extensions;
using TS3Bot.Core.Logging;
using TS3QueryLib.Net.Core.Server.Notification;

namespace TS3Bot.Core.Libraries
{
    public class Lang : Library
    {
        private const string defaultLang = "en";
        private string defaulClientLang = string.IsNullOrEmpty(Interface.TS3Bot.Config.Bot.DefaultLang) ? defaultLang : Interface.TS3Bot.Config.Bot.DefaultLang.ToLower();
        Dictionary<string, Dictionary<string, string>> _translations = new Dictionary<string, Dictionary<string, string>>();
        private ILogger log = Interface.TS3Bot.RootLogger;

        #region Initialization

        public Lang()
        {
        }

        #endregion Initialization

        #region Notifications

        public override void RegisterNotifications(NotificationHub notifications)
        {
        }

        #endregion Notifications

        public void RegisterMessages(Dictionary<string, string> translations, Extension ext, string lang = defaultLang)
        {
            string langFile = $"{ext.Name}-{lang}";
            _translations.TryAdd(langFile, translations);
        }

        public string GetMessage(string key, Extension ext, uint clid = 0)
        {
            if (string.IsNullOrEmpty(key) || ext == null) return key;

            return GetMessageKey(key, ext, GetLanguage(clid));
        }

        private string GetMessageKey(string key, Extension ext, string lang = defaultLang)
        {
            //lang = lang ?? defaultLang;
            string langFile = $"{ext.Name}-{lang}";

            Dictionary<string, string> trans = new Dictionary<string, string>();

            if (!_translations.TryGetValue(langFile, out trans))
            {
                log.Warning($"No translations found for '{ext.Name}' in language '{lang}'");
                // try get langs
                if (lang != defaultLang)
                {
                    langFile = $"{ext.Name}-{defaultLang}";
                    if (!_translations.TryGetValue(langFile, out trans))
                    {
                        log.Error($"No translations found for '{ext.Name}' in language '{defaultLang}'");
                    }
                }
            }

            string message;
            return trans.TryGetValue(key, out message) ? message : key;
        }

        public string GetLanguage(uint clid)
        {
            if (clid == 0)
            {
                return defaultLang;
            }
            var client = Server.GetClient(clid);
            // TODO: dodać możliwość wymuszenia języka przez klienta (wybrana opcja musiałaby być gdzieś zapisywana w db)
            if (client != null && !string.IsNullOrEmpty(client.ClientCountry)) return client.ClientCountry.ToLower();

            return defaulClientLang;
        }
    }
}
