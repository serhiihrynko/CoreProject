using API.Models;
using AutoMapper;
using Domain.Entities;

namespace API.Infrastructure.Automapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, CreateUserResponse>()
                .ForMember(dest => dest.UserId, source => source.MapFrom(src => src.Id));
        }
    }
}
