
using RabbitMq.Common.DTOs;

namespace RabbitMq.Console.Abstract
{
    internal interface IHttpClientService
    {
        Task<string> Login(string email, string password);
        Task<UserDto> GetCurrentUser();
        Task<List<UserDto>> GetAllUsers();
        Task SendNotification();
    }
}
