
using AutoMapper;
using Messaging.Application.DTOs;
using Messaging.Domain.Entities;

namespace Messaging.Application.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Chat, ChatDto>().ReverseMap();
            CreateMap<ChatUser, UserDto>().ReverseMap();
            CreateMap<Message, MessageDto>().ReverseMap();
        }
    }
}
