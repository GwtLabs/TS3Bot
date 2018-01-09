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
                cfg.CreateMap<ChannelDTO, ChannelData>();
                //cfg.CreateMap<ChannelDTO, ChannelData>().ForMember(dest => dest.StaffGroups, opt => opt.MapFrom(src => src.Groups));
                //cfg.CreateMap<GroupData, GroupDTO>();
            })
            .CreateMapper();
    }
}
