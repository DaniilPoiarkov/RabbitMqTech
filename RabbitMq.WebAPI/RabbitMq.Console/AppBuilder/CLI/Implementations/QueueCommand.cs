using Newtonsoft.Json;
using RabbitMq.Common.DTOs.NotificationsDto;
using RabbitMq.Console.Abstract;
using RabbitMq.Console.AppBuilder.AppContext;
using RabbitMq.Console.AppBuilder.CLI.Abstract;

namespace RabbitMq.Console.AppBuilder.CLI.Implementations
{
    internal sealed class QueueCommand : CliCommand
    {
        private readonly IHttpClientService _http;

        private static readonly string _baseUrl = "/api/queue";

        public override string ControllerName => "queue";
        public override string Description => "Allows you to communicate with Queue API\n\t" +
            "Options:\n\t\t" +
            "all - get all your notifications";

        public QueueCommand(IHttpClientService http)
        {
            _http = http;
        }

        public override async Task Execute(ConsoleAppContext context)
        {
            var args = context.Args;

            if(args.Length != 2 || args[1] != "all")
            {
                System.Console.WriteLine("No such command");
                return;
            }

            var response = await _http.GetRequest(_baseUrl + "?userId=" + context.User.Id);

            if (!await HandleResponse(response))
                return;

            var notifications = JsonConvert.DeserializeObject<List<NotificationDto>>(
                await response.Content.ReadAsStringAsync());

            if (!ValidateBody(notifications))
                return;

            System.Console.WriteLine("YOUR NOTIFICATIONS:");

            for (int i = 0; i < notifications?.Count; i++)
            {
                var notification = notifications[i];

                System.Console.WriteLine(
                    $"Id: {notification.Id}\n" +
                    $"Content: {notification.Content}\n" +
                    $"Recieved at: {notification.CreatedAt}\n");
            }
        }
    }
}
