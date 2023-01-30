using Quartz;
using Serilog;
using System.Reflection;

namespace RabbitMq.Services.Quartz.Jobs;

public class LogJob : IJob
{
    private readonly ILogger _logger;

    public LogJob(ILogger logger)
    {
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.Information("{job} is executed at {dateTime}", nameof(LogJob), DateTime.Now.ToLongTimeString());

        var path = Path.Combine(
            Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location) ?? Path.GetTempPath(),
            "..", "..", "..", "..", "Logs");

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        path = Path.Combine(path, $"Logs-{DateTime.Today.ToShortDateString()}.log");

        using var writer = new StreamWriter(path, true);
        var log = string.Format(
            "[{0}]|[INFO]|\t" +
            "Executing job of type {1}. " +
            "Result: {2}, " +
            "next execution will at {3}, " +
            "Fire instance Id: {4}",
            DateTime.Now.ToLongTimeString(),
            typeof(LogJob).Name,
            context.Result,
            context.NextFireTimeUtc,
            context.FireInstanceId);

        await writer.WriteLineAsync(log);
    }
}
