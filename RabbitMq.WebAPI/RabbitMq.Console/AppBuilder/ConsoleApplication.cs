using Microsoft.AspNetCore.SignalR.Client;
using RabbitMq.Common.DTOs;
using RabbitMq.Common.DTOs.AuxiliaryModels;
using RabbitMq.Console.Abstract;
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
        public List<ICliCommand> CliCommands { get; init; }

        internal UserDto CurrentUser { get; set; } = new();

        public ConsoleApplication(
            ICommandContainer commandContainer, 
            HttpClient httpClient, 
            List<ICliCommand> commands)
        {
            _commandContainer = commandContainer;
            _httpClient = httpClient;
            CliCommands = commands;
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
                    var input = Ext.Ask("");
                    var args = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    var isHandled = false;

                    for (int i = 0; i < CliCommands.Count; i++)
                    {
                        var command = CliCommands[i];

                        if (command.ControllerName == args[0])
                        {
                            await command.Execute(args, this);
                            isHandled = true;
                        }
                    }

                    if(!isHandled)
                        System.Console.WriteLine("No such command");

                }
                catch(Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }
        }

        private async Task SetUpUserData()
        {
            await Login();
            await SetCurrentUser();

            var connection = new HubConnectionBuilder()
                .WithUrl(_httpClient.BaseAddress?.ToString() + "notifications")
                .Build();

            connection.ConfigureHubConnection(_httpClient);
            await connection.StartAsync();
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
