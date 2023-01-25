using Quartz;
using Quartz.Impl;
using RabbitMq.Services.Quartz;
using RabbitMq.Services.Quartz.Jobs;

namespace RabbitMq.WebAPI.DependencyInjection.ServiceInstallers;

public class QuartzServiceInstaller : IServiceInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz();

        services.AddSingleton(sp =>
        {
            var scheduler = new StdSchedulerFactory().GetScheduler().Result;
            scheduler.JobFactory = new CustomJobFactory(sp);
            return scheduler;
        });

        services.AddTransient<LogJob>();
    }
}
