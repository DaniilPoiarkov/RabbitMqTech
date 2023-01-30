using Quartz;
using Serilog;
using Serilog.Events;
using System.Reflection;

namespace RabbitMq.Services.Quartz.Jobs;

public class ReadAndDeleteLogsJob : IJob
{
    private readonly ILogger _logger;

    public ReadAndDeleteLogsJob(ILogger logger)
    {
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        if (_logger.IsEnabled(LogEventLevel.Information))
            _logger.Information("{job} is executed at {dateTime}", nameof(ReadAndDeleteLogsJob), DateTime.UtcNow.ToLongTimeString());

        var date = DateTime.Today.AddDays(-1).ToShortDateString();

        var path = Path.Combine(
            Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location) ?? Path.GetTempPath(),
            "..", "..", "..", "..", "Logs", $"Logs-{date}.log");

        var text = string.Empty;
        var logs = new List<string>();

        if (!File.Exists(path))
        {
            if (_logger.IsEnabled(LogEventLevel.Information))
                _logger.Information("No logs for {date} date", date);

            return;
        }

        using var reader = new StreamReader(path);

        while ((text = await reader.ReadLineAsync()) != null)
        {
            logs.Add(text);
        }

        reader.Dispose();

        bool isDeletedSuccessfully;
        string errorMessage = string.Empty;

        try
        {
            File.Delete(path);
            isDeletedSuccessfully = true;
        }
        catch(Exception ex)
        {
            if (_logger.IsEnabled(LogEventLevel.Error))
                _logger.Error(
                    "Exception {exceptionType} while deleting file. Message: {exceptionMessage}",
                    ex.GetType().Name,
                    ex.Message);

            isDeletedSuccessfully = false;
            errorMessage = ex.Message;
        }

        if (_logger.IsEnabled(LogEventLevel.Information))
            _logger.Information("There\'re {count} logs for {Date} date. Deletion result: {isDeletedSuccessfully}",
                logs.Count, date, isDeletedSuccessfully ? "OK" : errorMessage);
    }
}
