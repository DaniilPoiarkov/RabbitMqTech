using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RabbitMq.Common.DTOs.AuxiliaryModels;
using RabbitMq.Common.Exceptions;
using RabbitMq.DAL;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RabbitMq.IntegrationTests
{
    public class IntegrationTests
    {
        protected readonly HttpClient HttpClient;

        public IntegrationTests()
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
                            options.UseInMemoryDatabase("IntegrationTestsDb:");
                        });
                    });
                });

            HttpClient = appFactory.CreateClient();
        }

        protected async Task Authenticate()
        {
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
        }

        private async Task<string> GetToken()
        {
            var response = await HttpClient.PostAsJsonAsync("/api/auth", new UserRegister()
            {
                Email = "test",
                Password = "test",
                Username = "test",
            });

            var registerResponse = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<AccessToken>(registerResponse);

            if (token?.Token == null)
                throw new UnreachableException(registerResponse);

            return token.Token;
        }
    }
}
