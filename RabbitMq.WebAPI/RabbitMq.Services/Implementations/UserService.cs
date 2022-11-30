using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RabbitMq.Common.DTOs;
using RabbitMq.Common.Entities;
using RabbitMq.Common.Exceptions;
using RabbitMq.DAL;
using RabbitMq.Services.Abstract;
using System.Threading;

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

        public async Task<UserDto> GetUserByEmail(string email, CancellationToken cancellationToken = default)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
            if (user == null)
                throw new NotFoundException(nameof(User));

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserById(int id, CancellationToken cancellationToken = default)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (user == null)
                throw new NotFoundException(nameof(User));

            return _mapper.Map<UserDto>(user);
        }

        public async Task SetConnectionId(string connectionId, int userId, CancellationToken cancellationToken = default)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null)
                throw new NotFoundException(nameof(User));

            user.ConnectionId = connectionId;
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
