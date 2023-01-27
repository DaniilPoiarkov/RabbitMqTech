using Quartz;
using Serilog;

namespace RabbitMq.Services.Quartz.Jobs;

public class LogJob : IJob
{
    private readonly ILogger _logger;

    public LogJob(ILogger logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.Information("{job} is executed at {dateTime}", nameof(LogJob), DateTime.UtcNow.ToLongTimeString());
        return Task.CompletedTask;
    }
}
