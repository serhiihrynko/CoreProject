using AutoMapper;

namespace API.Infrastructure.Automapper
{
    public class AutoMapperProfile : Profile
    {
        // use DI IMapper, _mapper.Map<TDestination>(source));

        public AutoMapperProfile()
        {
            //CreateMap<>()
            //    .ForMember(dest => dest, source => source.MapFrom(src => src));
        }
    }
}
