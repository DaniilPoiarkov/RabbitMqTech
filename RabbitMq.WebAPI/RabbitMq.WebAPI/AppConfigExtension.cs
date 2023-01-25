using Microsoft.EntityFrameworkCore;
using Quartz;
using RabbitMq.DAL;
using RabbitMq.Services.Quartz.Jobs;
using ILogger = Serilog.ILogger;

namespace RabbitMq.WebAPI;

public static class AppConfigExtension
{
    public static void ApplyPendingMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<RabbitMqDb>();

        if (db.Database.GetPendingMigrations().Any())
            db.Database.Migrate();
    }

    public static void StartQuartzJobs(this WebApplication app)
    {
        var scheduler = app.Services.GetService<IScheduler>();

        if (scheduler is null)
        {
            var logger = app.Services.GetRequiredService<ILogger>();
            logger.Error("Service {serviceName} cannot be registered. Quartz \'Jobs\' are not running", nameof(IScheduler));
            return;
        }

        scheduler.Start();
        scheduler.ScheduleJob(
            CreateJob<LogJob>(),
            TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule(s => s
                    .WithIntervalInSeconds(5)
                    .StartingDailyAt(
                        TimeOfDay.HourAndMinuteOfDay(0, 0))
                    .OnEveryDay())
                .Build());

        scheduler.ScheduleJob(
            CreateJob<SendReminderJob>(),
            TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule(s => s
                    .WithIntervalInMinutes(1)
                    .StartingDailyAt(
                        TimeOfDay.HourAndMinuteOfDay(0, 0))
                    .OnEveryDay())
                .Build());
    }

    private static IJobDetail CreateJob<TJob>()
        where TJob : IJob
    {
        return JobBuilder.Create<TJob>().Build();
    }
}
