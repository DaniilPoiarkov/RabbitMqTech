using MediatR;
using RabbitMq.Services.MediatoR.User.Requests;
using RabbitMq.WebAPI.Behaviors;

namespace RabbitMq.WebAPI.DependencyInjection.ServiceInstallers;

public class MediatRServiceInstaller : IServiceInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMediatR(typeof(GetUserByIdRequest).Assembly)
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    }
}
