using AutoMapper;
using TS3Bot.Ext.AutoPoke.Model;
using TS3Bot.Ext.AutoPoke.DTO;
using System.Collections.Generic;

namespace TS3Bot.Ext.AutoPoke.Mappers
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
            => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChannelDTO, ChannelData>();
                cfg.CreateMap<List<ChannelDTO>, List<ChannelData>>();
            })
            .CreateMapper();
    }
}
