
using RabbitMq.Common.DTOs;
using RabbitMq.Common.DTOs.AuxiliaryModels;

namespace RabbitMq.Identity.Abstract
{
    public interface IAuthService
    {
        Task<UserDto> Login(UserLogin credentials, CancellationToken token = default);
        string GetToken(UserDto user);
        Task<string> Register(UserRegister credentials, CancellationToken token = default);
        Task ResetPassword(ResetPasswordModel model, CancellationToken token = default);
    }
}
