using Microsoft.AspNetCore.SignalR;
using Quartz;
using RabbitMq.Broker.Hubs;
using RabbitMq.Services.Abstract;
using Serilog;
using Serilog.Events;

namespace RabbitMq.Services.Quartz.Jobs;

public class StatisticNotificationJob : IJob
{
    private readonly IHubContext<NotificationHub> _context;
    private readonly ILogger _logger;
    private readonly IQueueService _queueService;
    private readonly IUserService _userService;

    public StatisticNotificationJob(
        ILogger logger,
        IHubContext<NotificationHub> context,
        IQueueService queueService,
        IUserService userService)
    {
        _logger = logger;
        _context = context;
        _queueService = queueService;
        _userService = userService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        if (_logger.IsEnabled(LogEventLevel.Information))
            _logger.Information("Sending reminders for all users from {hubName} hub", nameof(NotificationHub));

        var users = await _userService.GetAllUsers(context.CancellationToken);
        var notifyings = new List<Task>(users.Count);

        for(int i = 0; i < users.Count; i++)
        {
            var user = users[i];

            notifyings.Add(_context.Clients.Client(user.ConnectionId).SendAsync(
                "recieveReminder",
                $"Hello, {user.Username}, you have {user.Notifications.Count} notifications!",
                context.CancellationToken));
        }

        await Task.WhenAll(notifyings);
    }
}
