using Microsoft.Extensions.Caching.Memory;
using RabbitMq.Common.DTOs;
using RabbitMq.Common.Entities;
using RabbitMq.Common.Exceptions;
using RabbitMq.Services.Abstract;
using Serilog;
using System.Diagnostics;

namespace RabbitMq.Services.Implementations
{
    public class DecoratedUserService : IUserService
    {
        private readonly UserService _decorated;
        private readonly IMemoryCache _cache;
        private readonly ILogger _logger;
        public DecoratedUserService(UserService service, IMemoryCache cache, ILogger logger)
        {
            _decorated = service;
            _cache = cache;
            _logger = logger;
        }
        public async Task<List<UserDto>> GetAllUsers(CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                return await _decorated.GetAllUsers(cancellationToken);
            }
            finally
            {
                _logger.Information(
                    "{methodName} took {duration}ms.",
                    typeof(DecoratedUserService).GetMethod(nameof(GetAllUsers))?.Name,
                    sw.ElapsedMilliseconds);
            }
        }

        public async Task<UserDto> GetUserByEmail(string email, CancellationToken cancellationToken = default) => 
            await _cache.GetOrCreateAsync(email, entry =>
            {
                _logger.Information("User wtih key {Key} not cached.", email);
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
                return _decorated.GetUserByEmail(email, cancellationToken);
            }) ?? throw new NotFoundException(nameof(User));

        public async Task<UserDto> GetUserById(int id, CancellationToken cancellationToken = default) =>
            await _cache.GetOrCreateAsync(id, entry =>
            {
                _logger.Information("User wtih key {Key} not cached.", id);
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
                return _decorated.GetUserById(id, cancellationToken);
            }) ?? throw new NotFoundException(nameof(User));

        public async Task SetConnectionId(string connectionId, int userId, CancellationToken cancellationToken = default) =>
            await _decorated.SetConnectionId(connectionId, userId, cancellationToken);
    }
}
