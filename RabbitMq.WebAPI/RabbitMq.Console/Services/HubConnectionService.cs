using Microsoft.AspNetCore.SignalR.Client;
using RabbitMq.Console.Abstract;
using RabbitMq.Console.Extensions;
using RabbitMq.Console.IoC.Options;
using RabbitMq.Console.Models;

namespace RabbitMq.Console.Services
{
    public class HubConnectionService : IHubConnectionService
    {
        private readonly HubConnection _hubConnection;

        public HubConnectionService(IOptions<HubOptions> options)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(options.Value.Url)
                .Build();
        }

        public async Task StartAsync() => await _hubConnection.StartAsync();

        public async Task StopAsync() => await _hubConnection.StopAsync();

        public void Configure(HttpClient httpClient) => _hubConnection.ConfigureHubConnection(httpClient);
    }
}
