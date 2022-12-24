using Microsoft.AspNetCore.SignalR.Client;
using RabbitMq.Console.Extensions;

namespace RabbitMq.Console.Services
{
    internal class HubConnectionService
    {
        private HubConnection _hubConnection { get; }

        public HubConnectionService(string url)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(url)
                .Build();
        }

        public async Task StartAsync() => await _hubConnection.StartAsync();

        public async Task StopAsync() => await _hubConnection.StopAsync();

        public void Configure(HttpClient httpClient) => _hubConnection.ConfigureHubConnection(httpClient);
    }
}
