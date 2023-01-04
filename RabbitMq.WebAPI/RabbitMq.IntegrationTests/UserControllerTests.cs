global using Newtonsoft.Json;
using RabbitMq.Common.DTOs;
using RabbitMq.Common.DTOs.AuxiliaryModels;
using RabbitMq.Common.Exceptions;
using System.Net.Http.Json;

namespace RabbitMq.IntegrationTests
{
    public class UserControllerTests : IntegrationTests
    {
        private static readonly string _baseUrl = "/api/user";

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

            if (user is null)
                throw new UnreachableException(await response.Content.ReadAsStringAsync());

            user.Username.Should().Be("test");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetUserById_WhenUserWithGivenIdNotExist_ThenStatusCodeShouldBeNotFound()
        {
            await Authenticate();

            var response = await HttpClient.GetAsync(_baseUrl + "?id=2");

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
                await response.Content.ReadAsStringAsync()) ?? throw new UnreachableException(
                    await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            user.Username.Should().Be("TestUser");
            user.Email.Should().Be("email");
        }

        [Fact]
        public async Task GetAllUsers_WhenOneUserExist_ThenReturnsCollectionWithOneUser()
        {
            await Authenticate();
            var response = await HttpClient.GetAsync(_baseUrl + "/all");
            var users = JsonConvert.DeserializeObject<List<UserDto>>(
                await response.Content.ReadAsStringAsync()) ?? throw new UnreachableException(
                    await response.Content.ReadAsStringAsync());

            users.Should().HaveCount(1);
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
                await getResponse.Content.ReadAsStringAsync()) ?? throw new UnreachableException(
                    await putResponse.Content.ReadAsStringAsync() + "\t---\t" + 
                    await getResponse.Content.ReadAsStringAsync());

            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            putResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            currentUser.ConnectionId.Should().Be("testconnectionid");
            currentUser.Username.Should().Be("test");
        }
    }
}
