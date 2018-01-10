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
                //cfg.CreateMap<ChannelDTO, ChannelData>()
                //.ConvertUsing(source => source.StaffGroups?.Select(p => new GroupData
                //{
                //    Ticker = source.Security?.Ticker,
                //    Open = p.Open,
                //    Close = p.Close
                //}).ToList()
                //);
            })
            .CreateMapper();
    }
}



//cfg.CreateMap<ChannelDTO, ChannelData>().ForMember(
//    dest => dest.StaffGroups,
//    opt => opt.MapFrom(src => src.StaffGroups){opt=>var  = new GroupData() }
//);
//cfg.CreateMap<ChannelDTO, ChannelData>().ForMember(
//    dest => dest.StaffGroups,
//    opt => opt.MapFrom(src => src.StaffGroups=() => {
//        return new List<GroupDTO>() {
//            { Id=src.Id },
//            { Id=src.Id }
//        }
//    })
//);

//dest => dest.StaffGroups,
//                opt => opt.MapFrom(src => () => { return new List<GroupData>() {
//                        { Id=src.Id },
//                        { Id=src.Id }
//                }});
//                cfg.CreateMap<GroupDTO, GroupData>();
//            })