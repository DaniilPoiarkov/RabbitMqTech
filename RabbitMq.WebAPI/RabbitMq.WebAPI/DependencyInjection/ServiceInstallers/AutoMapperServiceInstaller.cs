using RabbitMq.Services.MappingProfiles;

namespace RabbitMq.WebAPI.DependencyInjection.ServiceInstallers
{
    public class AutoMapperServiceInstaller : IServiceInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<UserProfile>();
                cfg.AddProfile<NotificationProfile>();
            });
        }
    }
}
