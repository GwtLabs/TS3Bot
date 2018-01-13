using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TS3Bot.Core.Model;
using TS3QueryLib.Net.Core.Server.Entitities;

namespace TS3Bot.Ext.AutoPoke.Model
{
    public class ChannelData
    {
        private Object StatusLock = new Object();
        public uint Id { get; }
        public bool NeedHelp { get; private set; } = false;
        public bool WasStaff { get; private set; } = false;
        public List<ClientData> Clients { get; } = new List<ClientData>();
        public List<StaffGroupData> StaffGroups { get; private set; } = new List<StaffGroupData>();

        public ChannelData(uint id)
        {
            Id = id;
        }

        public bool IsEmpty()
        {
            return Clients.Any();
        }

        private void SetDefaultStatus()
        {
            NeedHelp = false;
            WasStaff = false;
        }

        public void Join(Client client)
        {
            // client.ClientType:   0 - normal, 1 - query
            if (client.ClientType != 0)
            {
                return;
            }

            ClientData cld = new ClientData() { Client = client };
            lock (StatusLock)
            {
                if (StaffGroups.Exists(g => client.ServerGroups.Contains(g.Id)))
                {
                    WasStaff = true;
                    NeedHelp = false;
                    cld.IsStaff = true;
                }
                else
                {
                    if (!NeedHelp)
                    {
                        NeedHelp = true;
                    }
                }
            }

            Clients.Add(cld);
        }

        public void Left(uint clid)
        {
            Clients.RemoveAll(c => c.Id == clid);
            lock (StatusLock)
            {
                if (IsEmpty())
                {
                    SetDefaultStatus();
                }
            }
        }
    }
}
