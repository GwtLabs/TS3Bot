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

        public void Join(Client client)
        {
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

        public void Left(Client client)
        {
            ClientData clientData = Clients.Where(c => c.Id == client.ClientId).First();
            Clients.Remove(clientData);
            lock (StatusLock)
            {
                if (!Clients.Any())
                {
                    NeedHelp = false;
                    WasStaff = false;
                }
            }
        }
    }
}
