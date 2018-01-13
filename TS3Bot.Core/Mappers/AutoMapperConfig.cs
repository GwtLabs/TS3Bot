using AutoMapper;
using System.Collections.Generic;
using TS3Bot.Core.Model;
using TS3QueryLib.Net.Core.Common.Responses;
using TS3QueryLib.Net.Core.Server.Entitities;
using TS3QueryLib.Net.Core.Server.Notification.EventArgs;

namespace TS3Bot.Core.Mappers
{
    class AutoMapperConfig
    {
        public static IMapper Initialize()
            => new MapperConfiguration(cfg =>
            {
                //cfg.CreateMap<EntityListCommandResponse<ClientListEntry>, Dictionary<uint, Client>>();
                cfg.CreateMap<ChannelListEntry, Channel>();
                cfg.CreateMap<ClientListEntry, Client>();
                cfg.CreateMap<ClientJoinedEventArgs, Client>();
            })
            .CreateMapper();
    }
}
