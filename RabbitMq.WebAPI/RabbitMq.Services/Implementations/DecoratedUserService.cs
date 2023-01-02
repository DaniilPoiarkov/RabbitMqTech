using Microsoft.Extensions.Caching.Memory;
using RabbitMq.Common.DTOs;
using RabbitMq.Common.Entities;
using RabbitMq.Common.Exceptions;
using RabbitMq.Services.Abstract;

namespace RabbitMq.Services.Implementations
{
    public class DecoratedUserService : IUserService
    {
        private readonly UserService _decorated;
        private readonly IMemoryCache _cache;
        public DecoratedUserService(UserService service, IMemoryCache cache)
        {
            _decorated = service;
            _cache = cache;
        }
        public Task<List<UserDto>> GetAllUsers(CancellationToken cancellationToken = default) => 
            _decorated.GetAllUsers(cancellationToken);

        public async Task<UserDto> GetUserByEmail(string email, CancellationToken cancellationToken = default) => 
            await _cache.GetOrCreateAsync(email, entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
                return _decorated.GetUserByEmail(email, cancellationToken);
            }) ?? throw new NotFoundException(nameof(User));

        public async Task<UserDto> GetUserById(int id, CancellationToken cancellationToken = default) =>
            await _cache.GetOrCreateAsync(id, entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
                return _decorated.GetUserById(id, cancellationToken);
            }) ?? throw new NotFoundException(nameof(User));

        public async Task SetConnectionId(string connectionId, int userId, CancellationToken cancellationToken = default) =>
            await _decorated.SetConnectionId(connectionId, userId, cancellationToken);
    }
}
