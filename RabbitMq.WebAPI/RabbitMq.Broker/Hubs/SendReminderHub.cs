using Microsoft.AspNetCore.SignalR;

namespace RabbitMq.Broker.Hubs;

public class SendReminderHub : Hub
{
    public async Task SendReminder(string reminder = "Hello, reminder!")
    {
        await Clients.All.SendAsync("recieveReminder", reminder);
    }
}
