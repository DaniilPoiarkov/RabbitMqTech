using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;
using Quartz.Simpl;
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
            var options = sp.GetRequiredService<IOptions<QuartzOptions>>();

            scheduler.JobFactory = new MicrosoftDependencyInjectionJobFactory(sp, options);
            return scheduler;
        });

        services.AddTransient<LogJob>();
    }
}
