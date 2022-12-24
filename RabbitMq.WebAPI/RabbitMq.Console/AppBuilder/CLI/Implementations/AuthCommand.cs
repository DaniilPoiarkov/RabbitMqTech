using RabbitMq.Common.DTOs.AuxiliaryModels;
using RabbitMq.Console.Abstract;
using RabbitMq.Console.AppBuilder.AppContext;
using RabbitMq.Console.AppBuilder.CLI.Abstract;
using RabbitMq.Console.Extensions;
using RabbitMq.Console.Services;

namespace RabbitMq.Console.AppBuilder.CLI.Implementations
{
    internal class AuthCommand : CliCommand
    {
        private readonly IHttpClientService _httpClientService;

        private readonly HubConnectionService _hubConnectionService;

        private readonly CurrentUserService _currentUserService;

        private static readonly string _baseUrl = "/api/auth";

        public override string ControllerName => "auth";
        public override string Description => "Allows you to login with new account or register\n\t" +
            "Options:\n\t\t" +
            "login: login in another account\n\t\t" +
            "register: create new account";

        public AuthCommand(IHttpClientService httpClientService, HubConnectionService hubservice, CurrentUserService currentUserService)
        {
            _httpClientService = httpClientService;
            _hubConnectionService = hubservice;
            _currentUserService = currentUserService;
        }

        public override async Task Execute(ConsoleAppContext context) 
        {
            var args = context.Args;

            if(args.Length != 2)
            {
                System.Console.WriteLine("No such implementation");
                return;
            }

            if (args[1] == "login")
            {
                System.Console.Clear();

                await StopHubConnection();
                await _currentUserService.SetUpUserData();
            }
            else if (args[1] == "register")
                await HandleRegisterCommand();
            else
                System.Console.WriteLine("No such implementation");
        }

        private async Task HandleRegisterCommand()
        {
            System.Console.Clear();

            var login = Ext.Ask("Write your email");
            var name = Ext.Ask("Write your name");

            var password = Ext.Ask("Write your password");
            var repeatedPassword = Ext.Ask("Repeat your password");

            if(password != repeatedPassword || string.IsNullOrEmpty(password))
            {
                System.Console.WriteLine("Password not match or empty");
                Environment.Exit(0);
            }

            var body = _httpClientService.CreateRequestStringContent(new UserRegister()
            {
                Email = login,
                Password = password,
                Username = name,
            });

            var response = await _httpClientService.PostRequest(_baseUrl, body);

            if (!await HandleResponse(response))
                Environment.Exit(0);

            if (response.IsSuccessStatusCode)
            {
                await StopHubConnection();

                System.Console.Clear();
                System.Console.WriteLine("Status code: " + (int)response.StatusCode + "\n" +
                    "Now you can login with your credentials");

                await _currentUserService.SetUpUserData();
            }
        }

        private async Task StopHubConnection()
        {
            await _hubConnectionService.StopAsync();
        }
    }
}
