using System;
using System.Collections.Generic;
using System.Text;

namespace TS3Bot.Core.Model
{
    public class Client
    {
        public uint Id { get; set; }
        public List<uint> ServerGroups { get; set; }

    }
}
