using Auth.Application.DTOs;
using Auth.Persistence.Identity;
using AutoMapper;

namespace Auth.Persistence.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ApplicationUser, ApplicationUserDto>()
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom<AvatarUrlResolver>())
                .ReverseMap();
        }
    }
}
