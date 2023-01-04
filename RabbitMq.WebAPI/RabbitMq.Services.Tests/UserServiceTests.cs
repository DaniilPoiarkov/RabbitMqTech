using Microsoft.Extensions.Caching.Memory;
using RabbitMq.Services.Implementations;
using System.Diagnostics;

namespace RabbitMq.Services.Tests
{
    public class UserServiceTests
    {
        private readonly UserService _service;
        private readonly DecoratedUserService _decorated;

        private static readonly int _userId = 1;
        private static readonly string _userEmail = "email";

        public UserServiceTests()
        {
            var db = new RabbitMqDb(new DbContextOptionsBuilder<RabbitMqDb>()
                .UseInMemoryDatabase("TestDb-" + Guid.NewGuid())
                .Options);

            db.Users.Add(new()
            {
                Id = _userId,
                Email = _userEmail,
                Password = "password",
                ConnectionId = "connectionId",
                Salt = "salt",
                Username = "Username"
            });

            db.SaveChanges();

            var mapper = new MapperConfiguration(opt =>
            {
                opt.AddProfile<UserProfile>();
            }).CreateMapper();

            var cache = new MemoryCache(new MemoryCacheOptions());

            _service = new(db, mapper);
            _decorated = new DecoratedUserService(_service, cache);
        }

        [Fact]
        public async Task GetUserById_WhenUserExist_ThenReturnsUser()
        {
            var user = await _service.GetUserById(_userId);

            Assert.NotNull(user);
            Assert.True(user.Email == _userEmail);
        }

        [Fact]
        public async Task GetUserByEmail_WhenUserExist_ThenReturnsUser()
        {
            var user = await _service.GetUserByEmail(_userEmail);

            Assert.NotNull(user);
            Assert.True(user.Id == _userId);
        }

        [Fact]
        public async Task GetUserById_WhenCached_ThenReturnsUserFaster()
        {
            var sw = new Stopwatch();

            sw.Start();
            var firstResponse = await _decorated.GetUserById(_userId);
            sw.Stop();
            var firstTime = sw.ElapsedMilliseconds;

            sw.Restart();
            var secondResponse = await _decorated.GetUserById(_userId);
            sw.Stop();
            var secondTime = sw.ElapsedMilliseconds;

            Assert.Equal(firstResponse.Username, secondResponse.Username);
            Assert.True(secondTime < firstTime);
        }

        [Fact]
        public async Task GetUserByEmail_WhenUserNotExist_NotFoundExceptionThrown()
        {
            await Assert.ThrowsAsync<NotFoundException>(async () => await _decorated.GetUserByEmail("invalidEmail"));
        }

        [Fact]
        public async Task GetUserById_WhenUserNotExist_ThenNotFoundExceptionThrown()
        {
            await Assert.ThrowsAsync<NotFoundException>(async () => await _decorated.GetUserById(2));
        }
    }
}
