using System;
using System.Collections.Generic;
using System.Text;

namespace TS3Bot.Ext.AutoPoke.Model
{
    public class ClientData
    {
        private bool IsStaff { get; } = false;
        private uint Id { get; }

        public ClientData(uint id, bool isStaff = false)
        {
            Id = id;
            IsStaff = isStaff;
        }
    }
}
