using Newtonsoft.Json;
using RabbitMq.Common.DTOs.NotificationsDto;
using RabbitMq.Console.Abstract;
using RabbitMq.Console.AppBuilder.AppContext;
using RabbitMq.Console.AppBuilder.CLI.Abstract;
using RabbitMq.Console.Extensions;

namespace RabbitMq.Console.AppBuilder.CLI.Implementations
{
    internal class NotificationCommand : CliCommand
    {
        private readonly IHttpClientService _http;

        private static readonly string _baseUrlPrivateNotifications = "/api/PrivateNotification";

        private static readonly string _baseUrlSimpleNotifications = "/api/simpleNotification";

        public override string ControllerName => "notification";
        public override string Description => "Allows you to send notifications to users.\n\t\t" +
            "Should be written with second argument as \"simple\" or \"private\" to determine with which\n\t\ttype of notification you want to interact\n\t" +
            "Options:\n\t\t" +
            "get - get all your notifications of current type\n\t\t" +
            "delete [id] - delete notification with specific Id, which you can find in \"get\" implementation\n\t\t" +
            "send - send notification of specific type. It will ask you for content and for Id of user for whome to send";

        public NotificationCommand(IHttpClientService http)
        {
            _http = http;
        }

        public override async Task Execute(ConsoleAppContext context)
        {
            var args = context.Args;

            if(args.Length <= 2)
            {
                System.Console.WriteLine("No such implementation. Write help to see more information");
                return;
            }

            if (args[1] == "private")
                await HandlePrivateNotificationCommand(args, context);
            if (args[1] == "simple")
                await HandleSimpleNotificationCommand(args, context);
        }

        private async Task HandlePrivateNotificationCommand(string[] args, ConsoleAppContext context)
        {
            if(args.Length == 3 && args[2] == "get")
                await HandleGetCommand<PrivateNotificationDto>(_baseUrlPrivateNotifications, context.User.Id);
            else if (args.Length == 4 && args[2] == "delete")
                await HandleDeleteCommand(args, _baseUrlPrivateNotifications);
            else if(args.Length == 3 && args[2] == "send")
            {
                var content = Ext.Ask("Write content of notification");

                if(!int.TryParse(Ext.Ask("Write reciever Id."), out int userId))
                {
                    System.Console.WriteLine("Cannot set value as reciever Id");
                    return;
                }

                var notification = new PrivateNotificationDto()
                {
                    Content = content,
                    RecieverId = userId,
                    SenderId = context.User.Id,
                };

                await HandleSendCommand(notification, _baseUrlPrivateNotifications);
            }
            else
                System.Console.WriteLine("No such implementation");
        }

        private async Task HandleSimpleNotificationCommand(string[] args, ConsoleAppContext context)
        {
            if (args.Length == 3 && args[2] == "get")
                await HandleGetCommand<SimpleNotificationDto>(_baseUrlSimpleNotifications, context.User.Id);
            else if (args.Length == 4 && args[2] == "delete")
                await HandleDeleteCommand(args, _baseUrlSimpleNotifications);
            else if (args.Length == 3 && args[2] == "send")
            {
                var content = Ext.Ask("Write content of notification");

                if(!int.TryParse(Ext.Ask("Write reciever Id."), out var userId))
                {
                    System.Console.WriteLine("Cannot set value as recieve Id");
                    return;
                }

                var notification = new SimpleNotificationDto()
                {
                    Content = content,
                    RecieverId = userId,
                };

                await HandleSendCommand(notification, _baseUrlSimpleNotifications);
            }
            else
                System.Console.WriteLine("No such implementation");
        }

        private async Task HandleGetCommand<TNotificationType>(string controllerName, int userId)
            where TNotificationType : NotificationDto
        {
            var response = await _http.GetRequest(controllerName + "?userId=" + userId);

            if (!await HandleResponse(response))
                return;

            var notifications = JsonConvert.DeserializeObject<List<TNotificationType>>(
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
                    $"Recieved at: {notification.CreatedAt}");

                if(notification is PrivateNotificationDto privateNotification)
                    System.Console.WriteLine("SenderId: " + privateNotification.SenderId + "\n");
                else
                    System.Console.WriteLine("\n");
            }
        }

        private async Task HandleDeleteCommand(string[] args, string controllerName)
        {
            var response = await _http.DeleteRequest(controllerName + "?id=" + args[3]);

            if (!await HandleResponse(response))
                return;

            System.Console.WriteLine("StatusCode: " + (int)response.StatusCode);
        }

        private async Task HandleSendCommand<TNotification>(TNotification notification, string controllerName)
            where TNotification : NotificationDto
        {
            var body = _http.CreateRequestStringContent(notification);
            var response = await _http.PostRequest(controllerName, body);

            if (!await HandleResponse(response))
                return;
            System.Console.WriteLine("StatusCode: " + (int)response.StatusCode);
        }
    }
}
