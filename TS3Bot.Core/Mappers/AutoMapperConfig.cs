using AutoMapper;
//using TS3Bot.Core.Model;
//using TS3Bot.Core.DTO;

namespace TS3Bot.Core.Mappers
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
            => new MapperConfiguration(cfg =>
            {
                //cfg.CreateMap<Order, OrderCreateDTO>();
                //cfg.CreateMap<Order.IStatus, StatusDTO>();
            })
            .CreateMapper();
    }

}
