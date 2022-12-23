using Newtonsoft.Json;
using RabbitMq.Common.DTOs;
using RabbitMq.Common.Exceptions;
using RabbitMq.Console.Abstract;
using RabbitMq.Console.AppBuilder.AppContext;
using RabbitMq.Console.AppBuilder.CLI.Abstract;

namespace RabbitMq.Console.AppBuilder.CLI.Implementations
{
    internal sealed class UserCommand : CliCommand
    {
        private readonly IHttpClientService _http;

        private readonly static string _baseUrl = "/api/user";

        public override string ControllerName => "user";
        public override string Description => "Allows you to send requests to user API.\n\t" +
            "Options:\n\t\t" +
            "me - get your information\n\t\t" +
            "id [id] - get user by id\n\t\t" +
            "email [email] - get user by email\n\t\t" +
            "all - in case you have admin permission allows you to get all users information.";

        public UserCommand(IHttpClientService http)
        {
            _http = http;
        }

        public override async Task Execute(ConsoleAppContext context, ConsoleApplication app)
        {
            var args = context.Args;

            if (args.Length > 3 || args.Length <= 1)
            {
                System.Console.WriteLine("No such implementation");
                return;
            }

            if (args[1] == "me")
                await HandleMeCommand(app);
            if (args.Length == 3 && args[1] == "id")
                await HandleIdCommand(args[2]);
            if(args.Length == 3 && args[1] == "email")
                await HandleEmailCommand(args[2]);
            if (args[1] == "all")
                await HandleAllCommand();
        }

        private async Task HandleMeCommand(ConsoleApplication app)
        {
            var response = await _http.GetRequest(_baseUrl + "/current");

            if (!await HandleResponse(response))
                return;
            
            var currentUser = JsonConvert.DeserializeObject<UserDto>(
                await response.Content.ReadAsStringAsync());

            if (!ValidateBody(currentUser))
                return;

            System.Console.WriteLine("YOUR INFORMATION:\n" +
                "Id: " + currentUser?.Id + "\n" +
                "Username: " + currentUser?.Username + "\n" +
                "Email: " + currentUser?.Email + "\n" +
                "ConnectionId: " + currentUser?.ConnectionId);

            app.SetCurrentUser(currentUser ?? throw new UnreachableException());
        }

        private async Task HandleIdCommand(string id)
        {
            var response = await _http.GetRequest(_baseUrl + "?id=" + id);

            if (!await HandleResponse(response))
                return;

            var user = JsonConvert.DeserializeObject<UserDto>(
                await response.Content.ReadAsStringAsync());

            if(!ValidateBody(user)) 
                return;

            System.Console.WriteLine("USER INFORMATION:\n" +
                "Id: " + user?.Id + "\n" +
                "Username: " + user?.Username + "\n" +
                "Email: " + user?.Email);
        }

        private async Task HandleEmailCommand(string email)
        {
            var response = await _http.GetRequest(_baseUrl + "/email?email=" + email);
            if (!await HandleResponse(response))
                return;

            var user = JsonConvert.DeserializeObject<UserDto>(
                await response.Content.ReadAsStringAsync());

            if (!ValidateBody(user))
                return;

            System.Console.WriteLine("USER INFORMATION:\n" +
                "Id: " + user?.Id + "\n" +
                "Username: " + user?.Username + "\n" +
                "Email: " + user?.Email);
        }

        private async Task HandleAllCommand()
        {
            var response = await _http.GetRequest(_baseUrl + "/all");

            if (!await HandleResponse(response))
                return;

            var users = JsonConvert.DeserializeObject<List<UserDto>>(
                await response.Content.ReadAsStringAsync());

            if (!ValidateBody(users))
                return;

            for (int i = 0; i < users?.Count; i++)
            {
                var user = users[i];

                System.Console.WriteLine("USER INFORMATION:\n" +
                    "Id: " + user?.Id + "\n" +
                    "Username: " + user?.Username + "\n" +
                    "Email: " + user?.Email + "\n");
            }
        }
    }
}
