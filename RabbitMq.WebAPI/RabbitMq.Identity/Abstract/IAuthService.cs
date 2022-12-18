
using RabbitMq.Common.DTOs;
using RabbitMq.Common.DTOs.AuxiliaryModels;

namespace RabbitMq.Identity.Abstract
{
    public interface IAuthService
    {
        Task<UserDto> Login(UserLogin credentials);
        string GetToken(UserDto user);
        Task<string> Register(UserRegister credentials);
    }
}
