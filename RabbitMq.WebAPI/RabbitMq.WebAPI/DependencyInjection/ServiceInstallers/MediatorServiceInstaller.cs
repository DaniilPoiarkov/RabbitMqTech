using MediatR;
using RabbitMq.Services.MediatoR.User.Requests;

namespace RabbitMq.WebAPI.DependencyInjection.ServiceInstallers
{
    public class MediatorServiceInstaller : IServiceInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(typeof(GetUserByIdRequest).Assembly);
        }
    }
}
