using RabbitMq.Common.DTOs.AuxiliaryModels;
using RabbitMq.Common.Exceptions;
using RabbitMq.Console.Abstract;
using RabbitMq.Console.AppBuilder.CLI.Abstract;
using RabbitMq.Console.Extensions;

namespace RabbitMq.Console.AppBuilder.CLI.Implementations
{
    internal class AuthCommand : CliCommand
    {
        private readonly IHttpClientService _httpClientService;

        private static readonly string _baseUrl = "/api/auth";

        public override string ControllerName => "auth";
        public override string Description => "Allows you to login with new account or register\n\t" +
            "Options:\n\t\t" +
            "login: login in another account\n\t\t" +
            "register: create new account";

        public AuthCommand(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        public override async Task Execute(string[] args, ConsoleApplication app)
        {
            if(args.Length != 2)
            {
                System.Console.WriteLine("No such implementation");
                return;
            }

            if (args[1] == "login")
            {
                System.Console.Clear();

                await StopHubConnection(app);
                await app.SetUpUserData();
            }
            else if (args[1] == "register")
                await HandleRegisterCommand(app);
            else
                System.Console.WriteLine("No such implementation");
        }

        private async Task HandleRegisterCommand(ConsoleApplication app)
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
                await StopHubConnection(app);

                System.Console.Clear();
                System.Console.WriteLine("Status code: " + (int)response.StatusCode + "\n" +
                    "Now you can login with your credentials");

                await app.SetUpUserData();
            }
        }

        private static async Task StopHubConnection(ConsoleApplication app)
        {
            if (app.HubConnection is null)
                throw new UnreachableException();

            await app.HubConnection.StopAsync();
        }
    }
}
