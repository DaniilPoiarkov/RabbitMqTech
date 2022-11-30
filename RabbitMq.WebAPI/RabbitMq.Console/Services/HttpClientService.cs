using RabbitMq.Common.DTOs;
using RabbitMq.Console.Abstract;
using Newtonsoft.Json;
using RabbitMq.Common.DTOs.AuxiliaryModels;
using System.Text;
using RabbitMq.Common.Exceptions;

namespace RabbitMq.Console.Services
{
    internal class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;
        public HttpClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<List<UserDto>> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public async Task<UserDto> GetCurrentUser()
        {
            var response = await _httpClient.SendAsync(new(HttpMethod.Get, "/api/user/current"));

            var user = JsonConvert.DeserializeObject<UserDto>(
                await response.Content.ReadAsStringAsync());

            if (user == null)
                throw new NotFoundException("User is null");

            return user;
        }

        public async Task<string> Login(string email, string password)
        {
            var response = await _httpClient.SendAsync(new(HttpMethod.Put, "/api/auth")
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(new UserLogin()
                    {
                        Email = email,
                        Password = password,
                    }),
                    Encoding.UTF8,
                    "application/json"),
            });

            if (!response.IsSuccessStatusCode)
            {
                System.Console.WriteLine("Invalid credentials. Admin permission denied");
                Environment.Exit(0);
            }

            return await response.Content.ReadAsStringAsync();
        }

        public Task SendNotification()
        {
            throw new NotImplementedException();
        }
    }
}
