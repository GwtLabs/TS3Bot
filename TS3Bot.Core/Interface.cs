using System;
using System.Collections.Generic;
using System.Text;

namespace TS3Bot.Core
{
    public static class Interface
    {
        /// <summary>
        /// Gets the main Oxide mod instance
        /// </summary>
        public static TS3Bot TS3Bot { get; private set; }

        /// <summary>
        /// Initializes TS3Bot
        /// </summary>
        public static void Initialize()
        {
            // Create if not already created
            if (TS3Bot != null) return;
            TS3Bot = new TS3Bot();
            TS3Bot.Load();
        }
    }
}
