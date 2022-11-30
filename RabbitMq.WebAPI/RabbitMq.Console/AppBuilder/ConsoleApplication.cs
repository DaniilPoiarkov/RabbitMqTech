using RabbitMq.Common.DTOs;
using RabbitMq.Console.Abstract;
using RabbitMq.Console.Extensions;
using RabbitMq.Console.IoC.Abstract;
using System.Net.Http.Headers;

namespace RabbitMq.Console.AppBuilder
{
    internal class ConsoleApplication
    {
        private readonly ICommandContainer _commandContainer;

        private readonly HttpClient _httpClient;

        private UserDto CurrentUser { get; set; } = new();

        public ConsoleApplication(ICommandContainer commandContainer, HttpClient httpClient)
        {
            _commandContainer = commandContainer;
            _httpClient = httpClient;
            SetUpUserData().Wait();
        }

        public async Task Run()
        {
            await Task.Delay(0);
            System.Console.WriteLine($"{CurrentUser.Id}. {CurrentUser.Username}");
        }

        private async Task SetUpUserData()
        {
            await Login();
            await SetCurrentUser();
        }

        private async Task Login()
        {
            var email = Ext.Ask("Write login");
            var password = Ext.Ask("Write password");

            var httpService = _commandContainer.GetCommand<IHttpClientService>();
            var token = await httpService.Login(email, password);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task SetCurrentUser()
        {
            var httpService = _commandContainer.GetCommand<IHttpClientService>();
            CurrentUser = await httpService.GetCurrentUser();
        }
    }
}
