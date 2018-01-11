﻿using System;
using System.Collections.Generic;
using System.Text;
using TS3QueryLib.Net.Core.Server.Entitities;

namespace TS3Bot.Ext.AutoPoke.Model
{
    public class ChannelData
    {
        public uint Id { get; }
        public bool NeedHelp { get; private set; } = false;
        public bool WasStaff { get; private set; } = false;
        public List<ClientData> Clients { get; } = new List<ClientData>();
        public List<StaffGroupData> StaffGroups { get; set; } = new List<StaffGroupData>();

        public ChannelData(uint id)
        {
            Id = id;
        }

        public void Join(ClientListEntry client)
        {
            ClientData cld;
            if (StaffGroups.Exists(g => client.ServerGroups.Contains(g.Id)))
            {
                WasStaff = true;
                cld = new ClientData(id: client.ClientId, isStaff: true);
                NeedHelp = false;
            }
            else
            {
                cld = new ClientData(id: client.ClientId);
                if (!NeedHelp)
                {
                    NeedHelp = true;
                }
            }

            Clients.Add(cld);
        }
    }
}
