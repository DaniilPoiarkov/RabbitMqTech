using RabbitMq.Identity.IoC;
using RabbitMq.Identity.Options;

namespace RabbitMq.WebAPI.DependencyInjection.ServiceInstallers
{
    public class IdentityServiceInstaller : IServiceInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            var options = new JwtOptions();
            configuration.GetSection(nameof(JwtOptions)).Bind(options);

            services.RegisterIdentity(opt =>
            {
                opt.Issuer = options.Issuer;
                opt.Audience = options.Audience;
                opt.Key = options.Key;
                opt.ValidFor = options.ValidFor;
            });
        }
    }
}
