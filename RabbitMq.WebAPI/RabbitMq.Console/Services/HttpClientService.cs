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
                Content = CreateRequestStringContent(new UserLogin()
                {
                    Email = email,
                    Password = password,
                })
            });

            if (!response.IsSuccessStatusCode)
            {
                System.Console.WriteLine("Invalid credentials. Admin permission denied");
                Environment.Exit(0);
            }

            return await response.Content.ReadAsStringAsync();
        }

        public StringContent CreateRequestStringContent(object body) => 
            new(
                JsonConvert.SerializeObject(body),
                Encoding.UTF8,
                "application/json");

        public async Task<HttpResponseMessage> GetRequest(string url) => 
            await _httpClient.SendAsync(new(HttpMethod.Get, url));

        public async Task<HttpResponseMessage> PutRequest(string url, StringContent? body = null) =>
            await _httpClient.SendAsync(new(HttpMethod.Put, url)
            {
                Content = body,
            });

        public async Task<HttpResponseMessage> PostRequest(string url, StringContent? body = null) =>
            await _httpClient.SendAsync(new(HttpMethod.Post, url)
            {
                Content = body,
            });

        public async Task<HttpResponseMessage> DeleteRequest(string url) =>
            await _httpClient.DeleteAsync(url);

        public HttpClient HttpClient => _httpClient;
    }
}
