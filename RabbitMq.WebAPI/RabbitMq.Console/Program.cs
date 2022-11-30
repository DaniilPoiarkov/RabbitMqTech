using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using RabbitMq.Common.DTOs;
using RabbitMq.Common.DTOs.AuxiliaryModels;
using RabbitMq.Common.DTOs.NotificationsDto;
using RabbitMq.Common.Entities.Notifications;
using RabbitMq.Console.Abstract;
using RabbitMq.Console.AppBuilder;
using RabbitMq.Console.Extensions;
using RabbitMq.Console.Services;
using System.Net.Http.Headers;
using System.Text;

//var builder = new ConsoleApplicationBuilder();

//builder.ConfigureHttpClient(client =>
//{
//    client.BaseAddress = new("https://localhost:7036");
//});

//builder.Commands
//    .AddCommandTransient<IHttpClientService, HttpClientService>();

//var app = builder.Build();

//await app.Run();

//Environment.Exit(0);

using var http = new HttpClient()
{
    BaseAddress = new("https://localhost:7036")
};

var response = await http.SendAsync(new(HttpMethod.Put, "/api/auth")
{
    Content = new StringContent(
        JsonConvert.SerializeObject(new UserLogin()
        {
            Email = "email",
            Password = "password",
        }),
        Encoding.UTF8, 
        "application/json"),
});

var token = await response.Content.ReadAsStringAsync();
http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

response = await http.SendAsync(new(HttpMethod.Get, "/api/user/current"));

var user = JsonConvert.DeserializeObject<UserDto>(
    await response.Content.ReadAsStringAsync());

if (user == null)
    throw new Exception("User is null");

Console.WriteLine("Auth +, press any key to continue");
Console.ReadKey();

// Done in app builder +

response = await http.SendAsync(new(HttpMethod.Get, "/api/privateNotification?userId=" + user.Id));

var notifications = JsonConvert.DeserializeObject<List<PrivateNotificationDto>>(
    await response.Content.ReadAsStringAsync());

if (notifications == null)
    throw new Exception("Notifications are null");

for(int i = 0; i < notifications.Count; i++)
{
    Console.WriteLine(i + ". CreatedAt: " + notifications[i].CreatedAt + " N_Dto: " + notifications[i]?.Content);
    //response = await http.SendAsync(new(HttpMethod.Delete, "/api/simpleNotification?id=" + notifications[i].Id));
    //if (response.IsSuccessStatusCode)
    //    Console.WriteLine("Notification deleted\tStatus code: " + (int)response.StatusCode);
    //else
    //    Console.WriteLine(await response.Content.ReadAsStringAsync());
}

var connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:7036/notifications")
    .Build();

connection.ConfigureHubConnection(http);
await connection.StartAsync();

Console.WriteLine("W8 for connection update message and press enter");

response = await http.SendAsync(new(HttpMethod.Get, "/api/user/current"));

user = JsonConvert.DeserializeObject<UserDto>(
    await response.Content.ReadAsStringAsync());

if (user == null)
    throw new Exception("User is null");

Console.ReadKey();
Console.WriteLine("Write notification message. " +
    "It will be delivered to yourself. " +
    "To exit send empty string");

while (true)
{
    var message = Console.ReadLine();

    if (string.IsNullOrEmpty(message))
        break;

    if(int.TryParse(Ext.Ask(
        "1 - send as private,\n" +
        "2 - send as simple (default)\n" +
        "Not a number value will be ignored"), out var index))
    {
        if(index == 1)
        {
            var notification = new PrivateNotification()
            {
                Content = message,
                RecieverConnectionId = user.ConnectionId,
                RecieverId = user.Id,
                SenderId = user.Id,
            };

            response = await http.SendAsync(new(HttpMethod.Post, "/api/privateNotification")
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(notification),
                    Encoding.UTF8,
                    "application/json")
            });
        }
        else
        {
            var notification = new SimpleNotification()
            {
                Content = message,
                RecieverConnectionId = user.ConnectionId,
                RecieverId = user.Id,
            };

            response = await http.SendAsync(new(HttpMethod.Post, "/api/simpleNotification")
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(notification),
                    Encoding.UTF8,
                    "application/json")
            });
        }
    }

    if (!response.IsSuccessStatusCode)
        Console.WriteLine(
            await response.Content.ReadAsStringAsync() +
            "\n\tStatus code: " + (int)response.StatusCode);
}

Console.WriteLine("Press any key to exit");
Console.ReadKey();
