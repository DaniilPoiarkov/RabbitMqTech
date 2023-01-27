using RabbitMq.Common.DTOs;

namespace RabbitMq.Services.Abstract;

public interface IUserService
{
    Task<UserDto> GetUserById(int id, CancellationToken cancellationToken = default);
    Task<UserDto> GetUserByEmail(string email, CancellationToken cancellationToken = default);
    Task SetConnectionId(string connectionId, int userId, CancellationToken cancellationToken = default);
    Task<List<UserDto>> GetAllUsers(CancellationToken cancellationToken = default);
    Task UpdateAvatar(int userId, string avatarUrl, CancellationToken cancellationToken = default);
}
