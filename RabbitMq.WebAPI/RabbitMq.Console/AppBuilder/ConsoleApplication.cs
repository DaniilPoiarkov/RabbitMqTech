using Microsoft.AspNetCore.SignalR.Client;
using RabbitMq.Common.DTOs;
using RabbitMq.Console.Abstract;
using RabbitMq.Console.AppBuilder.AppContext;
using RabbitMq.Console.AppBuilder.CLI.Abstract;
using RabbitMq.Console.Extensions;
using RabbitMq.Console.IoC.Abstract;
using System.Net.Http.Headers;

namespace RabbitMq.Console.AppBuilder
{
    internal class ConsoleApplication
    {
        private readonly ICommandContainer _commandContainer;

        private readonly HttpClient _httpClient;

        private readonly List<Func<ConsoleAppContext, ConsoleAppContext>> _middlewares;

        public List<ICliCommand> CliCommands { get; init; }
        internal HubConnection? HubConnection { get; private set; }
        internal UserDto CurrentUser { get; set; } = new();

        public ConsoleApplication(
            ICommandContainer commandContainer, 
            HttpClient httpClient, 
            List<ICliCommand> commands,
            List<Func<ConsoleAppContext, ConsoleAppContext>> middlewares)
        {
            _commandContainer = commandContainer;
            _httpClient = httpClient;
            CliCommands = commands;
            _middlewares = middlewares;
        }

        public async Task Run()
        {
            await SetUpUserData();

            System.Console.WriteLine($"Login as:\n\t" +
                $"Id: {CurrentUser.Id},\n\t" +
                $"Name: {CurrentUser.Username}\n\t" +
                $"Email: {CurrentUser.Email}\n\n" +
                $"Write \"help\" to see available commands.");

            while (true)
            {
                try
                {
                    var args = Ext.Ask()
                        .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    var context = new ConsoleAppContext(args, CurrentUser);

                    for (int i = 0; i < _middlewares.Count; i++)
                        context = _middlewares[i].Invoke(context);

                    if (context.IsInterrupted)
                        continue;

                    var command = CliCommands
                        .FirstOrDefault(cli => cli.ControllerName == context.Args[0]);

                    if (command == null)
                    {
                        System.Console.WriteLine($"No command for \'{context.Args[0]}\' argument");
                        continue;
                    }

                    await command.Execute(context, this);

                }
                catch(Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }
        }

        internal async Task SetUpUserData()
        {
            await Login();
            await SetCurrentUser();

            HubConnection = new HubConnectionBuilder()
                .WithUrl(_httpClient.BaseAddress?.ToString() + "notifications")
                .Build();

            HubConnection.ConfigureHubConnection(_httpClient);
            await HubConnection.StartAsync();
        }

        private async Task Login()
        {
            var email = Ext.Ask("Write login");
            var password = Ext.Ask("Write password");

            var httpService = _commandContainer.GetCommand<IHttpClientService>();

            var token = await httpService.Login(email, password);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task SetCurrentUser() => CurrentUser = 
            await _commandContainer.GetCommand<IHttpClientService>().GetCurrentUser();

        internal void SetCurrentUser(UserDto currentUser) => CurrentUser = currentUser;
    }
}
