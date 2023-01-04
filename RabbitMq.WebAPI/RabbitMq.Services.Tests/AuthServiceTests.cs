using RabbitMq.Common.DTOs.AuxiliaryModels;
using RabbitMq.Identity.AuthServices;

namespace RabbitMq.Services.Tests
{
    public class AuthServiceTests
    {
        private readonly AuthService _service;
        public AuthServiceTests()
        {
            var db = new RabbitMqDb(
                new DbContextOptionsBuilder<RabbitMqDb>()
                .UseInMemoryDatabase("TestDb-" + Guid.NewGuid())
                .Options);

            var mapper = new MapperConfiguration(opt =>
            {
                opt.AddProfile<UserProfile>();
            }).CreateMapper();

            var jwt = new JwtTokenFactory(new()
            {
                Audience = "Test",
                Issuer = "Test",
                ValidFor = 1,
                Key = "SuperSecretKeyForTests",
            });

            _service = new(db, mapper, jwt);
        }

        [Fact]
        public async Task Register_WhenValidModel_ThenHashPassword()
        {
            var register = new UserRegister()
            {
                Email = "Test",
                Password = "Test",
                Username = "Test",
            };

            await _service.Register(register);

            var user = await _service.Login(new UserLogin()
            {
                Email = "Test",
                Password = "Test",
            });

            Assert.NotNull(user);
            Assert.NotEqual(register.Password, user.Password);
            Assert.Equal(register.Email, user.Email);
        }

        [Fact]
        public async Task Login_WhenInvalidPassword_ThenValidationExceptionThrows()
        {
            var register = new UserRegister()
            {
                Email = "Test",
                Password = "Test",
                Username = "Test",
            };

            await _service.Register(register);

            await Assert.ThrowsAsync<ValidationException>(async () => 
                await _service.Login(new UserLogin() 
                { 
                    Email = "Test",
                    Password = "invalid"
                }));
        }

        [Fact]
        public async Task Login_WhenValidCredentials_ThenReturnsUser()
        {
            var register = new UserRegister()
            {
                Email = "Test",
                Password = "Test",
                Username = "Test",
            };

            await _service.Register(register);
            var user = await _service.Login(new UserLogin()
            {
                Email = "Test",
                Password = "Test",
            });

            Assert.NotNull(user);
            Assert.Equal(user.Username, register.Username);
            Assert.Equal(user.Email, register.Email);
        }
    }
}
