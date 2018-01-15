using System.Collections.Generic;
using TS3Bot.Core.Extensions;
using TS3Bot.Core.Logging;
using TS3QueryLib.Net.Core.Server.Notification;

namespace TS3Bot.Core.Libraries
{
    public class Lang : Library
    {
        private string defaultLang = Interface.TS3Bot.Config.Bot.DefaultLang;
        Dictionary<string, Dictionary<string, string>> _translations = new Dictionary<string, Dictionary<string, string>>();

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

        public void RegisterMessages(Dictionary<string, string> translations, Extension ext, string lang = null)
        {
            lang = lang ?? defaultLang;
            string langFile = $"{ext.Name}-{lang}";
            _translations.TryAdd(langFile, translations);
        }

        public string GetMessage(string key, Extension ext, uint clid = 0)
        {
            if (string.IsNullOrEmpty(key) || ext == null) return key;

            return GetMessageKey(key, ext, GetLanguage(clid));
        }

        private string GetMessageKey(string key, Extension ext, string lang = null)
        {
            lang = lang ?? defaultLang;
            string langFile = $"{ext.Name}-{lang}";

            Dictionary<string, string> trans = new Dictionary<string, string>();

            if (!_translations.TryGetValue(langFile, out trans))
            {
                Interface.TS3Bot.LogWarning(lang);
                // try get langs
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
            if (client != null && !string.IsNullOrEmpty(client.ClientCountry)) return client.ClientCountry.ToLower();

            return defaultLang;
        }
    }
}
