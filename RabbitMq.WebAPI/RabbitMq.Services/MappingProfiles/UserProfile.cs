using AutoMapper;
using RabbitMq.Common.DTOs;
using RabbitMq.Common.Entities;

namespace RabbitMq.Services.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
