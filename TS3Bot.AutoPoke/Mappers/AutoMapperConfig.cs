using AutoMapper;
using TS3Bot.Ext.AutoPoke.Model;
using TS3Bot.Ext.AutoPoke.DTO;

namespace TS3Bot.Ext.AutoPoke.Mappers
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
            => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChannelData, ChannelDTO>();
                //cfg.CreateMap<Group, GroupDTO>();
            })
            .CreateMapper();
    }
}
