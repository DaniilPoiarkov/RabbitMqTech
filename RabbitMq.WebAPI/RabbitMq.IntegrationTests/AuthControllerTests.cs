using RabbitMq.Common.DTOs.AuxiliaryModels;
using System.Net.Http.Json;

namespace RabbitMq.IntegrationTests
{
    public class AuthControllerTests : IntegrationTests
    {
        private static readonly string _baseUrl = "/api/auth";
        private static readonly UserRegister _registerModel = new()
        {
            Email = "test",
            Username = "test",
            Password = "test",
        };
        private static readonly UserLogin _loginModel = new()
        {
            Email = "test",
            Password = "test",
        };

        [Fact]
        public async Task Register_WhenValidData_ThenStatusCodeOkAndReturnsToken()
        {
            var response = await HttpClient.PostAsJsonAsync(_baseUrl, _registerModel);

            var body = JsonConvert.DeserializeObject<AccessToken>(
                await response.Content.ReadAsStringAsync());

            Assert.NotNull(body);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Register_WhenUserWithGivenEmailExist_ThenStatusCodeBadRequest()
        {
            await HttpClient.PostAsJsonAsync(_baseUrl, _registerModel);
            var response = await HttpClient.PostAsJsonAsync(_baseUrl, _registerModel);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Login_WhenUserExist_ThenStatusCodeOkAndTokenReturns()
        {
            await HttpClient.PostAsJsonAsync(_baseUrl, _registerModel);
            var response = await HttpClient.PutAsJsonAsync(_baseUrl, _loginModel);

            var body = JsonConvert.DeserializeObject<AccessToken>(
                await response.Content.ReadAsStringAsync());

            body.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Login_WhenLoginSuccessfully_ThenHeadersShouldContainLastTimeActionValueAsUtcNow()
        {
            await HttpClient.PostAsJsonAsync(_baseUrl, _registerModel);
            var response = await HttpClient.PutAsJsonAsync(_baseUrl, _loginModel);

            response.Headers.TryGetValues("LastAction", out var headerValues);

            Assert.NotNull(headerValues);
            headerValues.Should().ContainSingle();
            headerValues.ElementAt(0).Should().Be(DateTime.UtcNow.ToShortDateString());
        }
    }
}
