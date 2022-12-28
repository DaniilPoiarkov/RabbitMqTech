
namespace RabbitMq.Console.Abstract
{
    public interface IHubConnectionService
    {
        Task StartAsync();
        Task StopAsync();
        void Configure(HttpClient httpClient);
    }
}
