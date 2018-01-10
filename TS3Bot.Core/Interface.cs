using System;
using System.Collections.Generic;
using System.Text;
using TS3Bot.Core.Model;

namespace TS3Bot.Core
{
    class Interface
    {
        private static readonly Interface instance = new Interface();

        public static Server Server { get; private set; }
        
        static Interface()
        {
        }

        public static Interface Instance
        {
            get
            {
                return instance;
            }
        }

        private Interface()
        {

        }

        public void Initialize(Server server)
        {
            Server = server;
        }
    }
}
