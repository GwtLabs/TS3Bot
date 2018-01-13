using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TS3Bot.Core.Libraries;
using TS3Bot.Core.Model;
using TS3QueryLib.Net.Core.Server.Notification;

namespace TS3Bot.Core.Extensions
{
    public abstract class Extension
    {
        public abstract void RegisterNotifications(NotificationHub notifications);

        protected abstract void LoadDefaultConfig();

        public abstract string Name { get; }

        public static Server Server = Interface.TS3Bot.GetLibrary<Server>();

        public static Lang lang = Interface.TS3Bot.GetLibrary<Lang>();

        protected virtual void LoadDefaultMessages() { }

        private static IConfig config;

        protected void SaveConfig()
        {
            ExtConfig.Write(Name, config);
        }

        protected T GetConfig<T>()
        {
            if (config == null)
            {
                InitConfig<T>();
            }
            return (T)config;
        }

        protected void SetConfig(IConfig cfg)
        {
            config = cfg;
        }

        private void InitConfig<T>()
        {
            if (!ExtConfig.Exists(Name))
            {
                LoadDefaultConfig();
                SaveConfig();
            }
            config = (IConfig)ExtConfig.Read<T>(Name);
        }

        public class ExtConfig
        {
            public static T Read<T>(string name)
            {
                return JsonConvert.DeserializeObject<T>(System.IO.File.ReadAllText(Path(name)));
            }

            public static void Write(string name, IConfig config)
            {
                System.IO.File.WriteAllText(Path(name), JsonConvert.SerializeObject(config, Formatting.Indented));
            }

            public static bool Exists(string name)
            {
                return System.IO.File.Exists(Path(name));
            }

            private static string Path(string name)
            {
                return $"ts3bot.{name.ToLower()}.config.json";
            }
        }
    }
}
