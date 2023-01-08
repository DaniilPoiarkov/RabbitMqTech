using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RabbitMq.Common.DTOs;
using RabbitMq.Common.DTOs.AuxiliaryModels;
using RabbitMq.Common.Entities;
using RabbitMq.Common.Exceptions;
using RabbitMq.DAL;
using RabbitMq.Identity.Abstract;

namespace RabbitMq.Identity.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly RabbitMqDb _db;
        private readonly JwtTokenFactory _jwtFactory;
        private readonly IMapper _mapper;
        public AuthService(RabbitMqDb db, IMapper mapper, JwtTokenFactory factory)
        {
            _db = db;
            _mapper = mapper;
            _jwtFactory = factory;
        }

        public string GetToken(UserDto user) => 
            _jwtFactory.GenerateToken(user.Email, user.Username, user.Id);

        public async Task<UserDto> Login(UserLogin credentials, CancellationToken token = default)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == credentials.Email, token);

            if (user == null)
                throw new NotFoundException(nameof(User));

            if (!AuthHelper.ValidatePassword(credentials.Password, user.Salt, user.Password))
                throw new ValidationException("Invalid password");

            return _mapper.Map<UserDto>(user);
        }

        public async Task<string> Register(UserRegister credentials, CancellationToken token = default)
        {
            ValidateModelOrThrow(credentials);

            var salt = Convert.ToBase64String(AuthHelper.GetBytes());

            var user = new User()
            {
                Email = credentials.Email,
                Salt = salt,
                Password = AuthHelper.HashPassword(credentials.Password, salt),
                Username = credentials.Username,
            };

            await _db.Users.AddAsync(user, token);
            await _db.SaveChangesAsync(token);

            return GetToken(
                _mapper.Map<UserDto>(user));
        }

        public async Task ResetPassword(ResetPasswordModel model, CancellationToken token = default)
        {
            var user = await _db.Users.Where(u => u.Email == model.Email)
                .FirstOrDefaultAsync(token) ?? throw new NotFoundException(nameof(User));

            user.Salt = Convert.ToBase64String(AuthHelper.GetBytes());
            user.Password = AuthHelper.HashPassword(model.NewPassword, user.Salt);

            await _db.SaveChangesAsync(token);
        }

        private void ValidateModelOrThrow(UserRegister model)
        {
            if (string.IsNullOrEmpty(model.Password))
                throw new ValidationException("Password cannot be empty");
            if (string.IsNullOrEmpty(model.Username))
                throw new ValidationException("Username cannot be empty");
            if (string.IsNullOrEmpty(model.Email))
                throw new ValidationException("Email cannot be empty");
            if(_db.Users.Any(u => u.Email == model.Email))
                throw new ValidationException("Email is already used");
        }
    }
}
