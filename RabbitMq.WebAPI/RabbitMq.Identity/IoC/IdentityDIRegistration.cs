using Microsoft.Extensions.DependencyInjection;
using RabbitMq.Identity.Options;

namespace RabbitMq.Identity.IoC
{
    public static class IdentityDIRegistration
    {
        public static IServiceCollection RegisterIdentity(this IServiceCollection services, Action<JwtOptions> options)
        {
            var jwtOptions = new JwtOptions();
            options.Invoke(jwtOptions);
            return services.ApplyJwtConfiguration(jwtOptions);
        }
    }
}
