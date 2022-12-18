using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using RabbitMq.Common.DTOs;
using RabbitMq.Common.Entities.Notifications;

namespace RabbitMq.Console.Extensions
{
    internal static class HubConnectionExt
    {
        public static void ConfigureHubConnection(this HubConnection connection, HttpClient http)
        {
            connection.On("connected", async (string connectionId) =>
            {
                await http.SendAsync(new(HttpMethod.Put, "/api/user?connectionId=" + connectionId));

                var response = await http.SendAsync(new(HttpMethod.Get, "/api/user/current"));

                var user = JsonConvert.DeserializeObject<UserDto>(
                    await response.Content.ReadAsStringAsync());

                System.Console.WriteLine("ConnectionId updated:\t" +
                    $"{user?.ConnectionId}");
            });

            connection.On("PrivateNotification", (PrivateNotification notification) =>
            {
                System.Console.WriteLine("Recieved private notification:\n\t" +
                    notification.Content);
            });

            connection.On("SimpleNotification", (SimpleNotification notification) =>
            {
                System.Console.WriteLine("Recieved simple notification:\n\t" +
                    notification.Content);
            });
        }
    }
}
