using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RabbitMq.Common.DTOs;
using RabbitMq.Common.Entities;
using RabbitMq.Common.Exceptions;
using RabbitMq.DAL;
using RabbitMq.Services.Abstract;
using System.Linq;

namespace RabbitMq.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly RabbitMqDb _db;
        private readonly IMapper _mapper;
        public UserService(RabbitMqDb db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> GetAllUsers(CancellationToken cancellationToken = default) => 
            _mapper.Map<List<UserDto>>(
                await _db.Users.Include(u => u.Notifications)
                    .ToListAsync(cancellationToken));

        public async Task<UserDto> GetUserByEmail(string email, CancellationToken cancellationToken = default)
        {
            var user = await _db.Users.Where(u => u.Email == email)
                .Include(u => u.Notifications)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                throw new NotFoundException(nameof(User));

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserById(int id, CancellationToken cancellationToken = default)
        {
            var user = await _db.Users.Where(x => x.Id == id)
                .Include(u => u.Notifications)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                throw new NotFoundException(nameof(User));

            return _mapper.Map<UserDto>(user);
        }

        public async Task SetConnectionId(string connectionId, int userId, CancellationToken cancellationToken = default) =>
            await _db.Users.Where(u => u.Id == userId)
                .ExecuteUpdateAsync(c => c.SetProperty(
                    u => u.ConnectionId, p => connectionId), cancellationToken);
    }
}
