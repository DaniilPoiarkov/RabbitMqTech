using Quartz;
using Quartz.Simpl;
using Quartz.Spi;
using RabbitMq.Common.Exceptions;

namespace RabbitMq.Services.Quartz;

public class CustomJobFactory : SimpleJobFactory
{
    private readonly IServiceProvider _services;

    public CustomJobFactory(IServiceProvider services)
    {
        _services = services;
    }

    public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        var job = _services.GetService(bundle.JobDetail.JobType) ??
            throw new ServiceImplementationException($"Cannot create job of type {bundle.JobDetail.JobType.Name}");

        return (IJob)job;
    }
}
