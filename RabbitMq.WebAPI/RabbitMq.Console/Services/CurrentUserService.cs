using RabbitMq.Common.DTOs;
using RabbitMq.Console.Abstract;
using RabbitMq.Console.Extensions;
using System.Net.Http.Headers;

namespace RabbitMq.Console.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly HttpClient _httpClient;

        private readonly IHttpClientService _httpService;

        private readonly IHubConnectionService _hubConnectionService;
        public UserDto? CurrentUser { get; set; }

        public CurrentUserService(
            HttpClient httpClient,
            IHubConnectionService hubConnectionService,
            IHttpClientService httpService)
        {
            _httpClient = httpClient;
            _hubConnectionService = hubConnectionService;
            _httpService = httpService;
        }

        public void SetCurrentUser(UserDto currentUser) => CurrentUser = currentUser;

        public async Task SetCurrentUserFromRequest() => 
            CurrentUser = await _httpService.GetCurrentUser();

        public async Task SetUpUserData()
        {
            await Login();
            await SetCurrentUserFromRequest();

            await _hubConnectionService.StartAsync();
        }

        public async Task Login()
        {
            var email = Ext.Ask("Write login");
            var password = Ext.Ask("Write password");

            var token = await _httpService.Login(email, password);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
