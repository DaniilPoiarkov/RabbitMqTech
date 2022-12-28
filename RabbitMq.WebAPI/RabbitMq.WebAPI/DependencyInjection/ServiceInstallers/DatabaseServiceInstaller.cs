using Microsoft.EntityFrameworkCore;
using RabbitMq.DAL;

namespace RabbitMq.WebAPI.DependencyInjection.ServiceInstallers
{
    public class DatabaseServiceInstaller : IServiceInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetConnectionString(nameof(RabbitMqDb));

            services.AddDbContext<RabbitMqDb>(o => o.UseNpgsql(connection,
                b => b.MigrationsAssembly(typeof(RabbitMqDb).Assembly.FullName))
            .EnableDetailedErrors());
        }
    }
}
