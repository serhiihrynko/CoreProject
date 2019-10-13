using API.Models;
using AutoMapper;
using Domain.Entities;

namespace API.Infrastructure.Automapper
{
    public class AutoMapperProfile : Profile
    {
        // use DI IMapper, _mapper.Map<TDestination>(source));

        public AutoMapperProfile()
        {
            //CreateMap<>()
            //    .ForMember(dest => dest, source => source.MapFrom(src => src));

            CreateMap<User, CreateUserResponse>()
                .ForMember(dest => dest.UserId, source => source.MapFrom(src => src.Id));
        }
    }
}
