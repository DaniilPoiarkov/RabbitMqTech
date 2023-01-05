using RabbitMq.Common.DTOs;
using RabbitMq.Common.DTOs.AuxiliaryModels;
using System.Net.Http.Json;

namespace RabbitMq.IntegrationTests
{
    public class UserControllerTests : IntegrationTests
    {
        private static readonly string _baseUrl = "/api/user";

        public UserControllerTests() : base()
        {
            var appFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll<RabbitMqDb>();

                        var options = services.FirstOrDefault(descriptor =>
                            descriptor.ServiceType == typeof(DbContextOptions<RabbitMqDb>));

                        if (options is not null)
                            services.Remove(options);

                        services.AddDbContext<RabbitMqDb>(options =>
                        {
                            options.UseInMemoryDatabase("IntegrationTestsDb:User");
                        });
                    });
                });

            HttpClient = appFactory.CreateClient();

            _ = HttpClient.PostAsJsonAsync("/api/auth", RegisterModel).Result;
        }

        [Fact]
        public async Task AccessToController_WhenUserNotAuthenticated_ShouldReturnStatusCodeUnauthorize()
        {
            var response = await HttpClient.GetAsync(_baseUrl);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetCurrentUser_WhenCalls_ReturnsCurrentUser()
        {
            await Authenticate();

            var response = await HttpClient.GetAsync(_baseUrl + "/current");

            var user = JsonConvert.DeserializeObject<UserDto>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(user);
            user.Username.Should().Be("test");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetUserById_WhenUserWithGivenIdNotExist_ThenStatusCodeShouldBeNotFound()
        {
            await Authenticate();

            var response = await HttpClient.GetAsync(_baseUrl + "?id=3");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetUserByEmail_WhenUserExist_ThenReturnsUserWithGivenEmail()
        {
            await Authenticate();

            await HttpClient.PostAsJsonAsync("api/auth", new UserRegister()
            {
                Email = "email",
                Password = "password",
                Username = "TestUser",
            });

            var response = await HttpClient.GetAsync(_baseUrl + "/email?email=email");

            var user = JsonConvert.DeserializeObject<UserDto>(
                await response.Content.ReadAsStringAsync());

            Assert.NotNull(user);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            user.Username.Should().Be("TestUser");
            user.Email.Should().Be("email");
        }

        [Fact]
        public async Task GetAllUsers_WhenTwoUsersExist_ThenReturnsCollectionWithTwoUsers()
        {
            await Authenticate();
            var response = await HttpClient.GetAsync(_baseUrl + "/all");
            var users = JsonConvert.DeserializeObject<List<UserDto>>(
                await response.Content.ReadAsStringAsync());

            Assert.NotNull(users);
            users.Should().HaveCount(2);
            users[0].Username.Should().Be("test");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task SetConnectionId_WhenUpdateConnection_ThenReturnsNoContentAndUpdateUserConnectionId()
        {
            await Authenticate();
            var putResponse = await HttpClient.PutAsync(_baseUrl + "?connectionId=testconnectionid", null);

            var getResponse = await HttpClient.GetAsync(_baseUrl + "/current");
            var currentUser = JsonConvert.DeserializeObject<UserDto>(
                await getResponse.Content.ReadAsStringAsync());

            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            putResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            Assert.NotNull(currentUser);
            currentUser.ConnectionId.Should().Be("testconnectionid");
            currentUser.Username.Should().Be("test");
        }
    }
}
