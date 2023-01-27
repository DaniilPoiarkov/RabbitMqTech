using Microsoft.AspNetCore.SignalR;
using Quartz;
using RabbitMq.Broker.Hubs;
using Serilog;
using Serilog.Events;

namespace RabbitMq.Services.Quartz.Jobs;

public class SendReminderJob : IJob
{
    private readonly ILogger _logger;

    private readonly IHubContext<NotificationHub> _context;

    public SendReminderJob(IHubContext<NotificationHub> context, ILogger logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        if(_logger.IsEnabled(LogEventLevel.Information))
            _logger.Information("Send reminder to {hubName} hub", nameof(NotificationHub));

        await _context.Clients.All.SendAsync("recieveReminder", "Don\'t forget to have a rest!");
    }
}
