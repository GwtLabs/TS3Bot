using System;
using System.Collections.Generic;
using System.Text;

namespace TS3Bot.Ext.AutoPoke.Model
{
    public class Client
    {
        private bool IsStaff { get; } = false;
        private uint Id { get; }

        public Client(uint id)
        {
            Id = id;
        }
    }
}
