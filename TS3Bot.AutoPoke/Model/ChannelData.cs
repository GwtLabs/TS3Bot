using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TS3Bot.Core.Model;

namespace TS3Bot.Ext.AutoPoke.Model
{
    public class ChannelData
    {
        private Object StatusLock = new Object();
        public uint Id { get; }
        public bool NeedHelp { get; private set; } = false;
        public DateTime NeedHelpAt { get; private set; } = DateTime.MinValue;
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
            NeedHelpAt = DateTime.MinValue;
            NeedHelp = false;
            WasStaff = false;
        }

        public void Join(ClientData cd)
        {
            // client.ClientType:   0 - normal, 1 - query
            if (cd.Object.ClientType != 0)
            {
                return;
            }

            lock (StatusLock)
            {
                if (IsStaff(cd.Object))
                {
                    WasStaff = true;
                    NeedHelp = false;
                }
                else
                {
                    if (!NeedHelp)
                    {
                        NeedHelp = true;
                    }
                }
            }

            Clients.Add(cd);
        }

        public bool IsStaff(Client client)
        {
            return StaffGroups.Exists(g => client.ServerGroups.Contains(g.Id));
        }

        public void Left(uint clid)
        {
            Clients.RemoveAll(c => c.Object.ClientId == clid);
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
