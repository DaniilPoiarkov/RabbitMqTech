using RabbitMq.Common.Parameters;
using RabbitMq.Services.Abstract;
using RabbitMq.Services.Implementations;

namespace RabbitMq.WebAPI.DependencyInjection.ServiceInstallers
{
    public class ApplicationServiceInstaller : IServiceInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSignalR();
            services.AddLogging();

            services
                .AddTransient<IUserService, UserService>()
                .AddTransient<IQueueService, QueueService>()

                .AddScoped(typeof(UserParameters));
        }
    }
}
