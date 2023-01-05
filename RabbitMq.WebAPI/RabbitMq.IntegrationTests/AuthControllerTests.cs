using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RabbitMq.Common.DTOs.AuxiliaryModels;
using RabbitMq.DAL;
using System.Net.Http.Json;

namespace RabbitMq.IntegrationTests
{
    public class AuthControllerTests : IntegrationTests
    {
        private static readonly string _baseUrl = "/api/auth";

        public AuthControllerTests()
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
                            options.UseInMemoryDatabase("IntegrationTestsDb:Auth");
                        });
                    });
                });

            HttpClient = appFactory.CreateClient();
        }

        [Fact]
        public async Task Register_WhenValidData_ThenStatusCodeOkAndReturnsToken()
        {
            var response = await HttpClient.PostAsJsonAsync(_baseUrl, RegisterModel);

            var body = JsonConvert.DeserializeObject<AccessToken>(
                await response.Content.ReadAsStringAsync());

            Assert.NotNull(body);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Register_WhenUserWithGivenEmailExist_ThenStatusCodeBadRequest()
        {
            await HttpClient.PostAsJsonAsync(_baseUrl, RegisterModel);
            var response = await HttpClient.PostAsJsonAsync(_baseUrl, RegisterModel);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Login_WhenUserExist_ThenStatusCodeOkAndTokenReturns()
        {
            await HttpClient.PostAsJsonAsync(_baseUrl, RegisterModel);
            var response = await HttpClient.PutAsJsonAsync(_baseUrl, LoginModel);

            var body = JsonConvert.DeserializeObject<AccessToken>(
                await response.Content.ReadAsStringAsync());

            body.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Login_WhenLoginSuccessfully_ThenHeadersShouldContainLastTimeActionValueAsUtcNow()
        {
            await HttpClient.PostAsJsonAsync(_baseUrl, RegisterModel);
            var response = await HttpClient.PutAsJsonAsync(_baseUrl, LoginModel);

            response.Headers.TryGetValues("LastAction", out var headerValues);

            Assert.NotNull(headerValues);
            headerValues.Should().ContainSingle();
            headerValues.ElementAt(0).Should().Be(DateTime.UtcNow.ToShortDateString());
        }
    }
}
