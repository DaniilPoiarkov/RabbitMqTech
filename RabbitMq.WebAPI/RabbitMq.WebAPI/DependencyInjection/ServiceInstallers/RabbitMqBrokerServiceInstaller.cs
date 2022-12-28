using RabbitMq.Broker.DIRegistration;
using RabbitMq.Broker.Models.Options;

namespace RabbitMq.WebAPI.DependencyInjection.ServiceInstallers
{
    public class RabbitMqBrokerServiceInstaller : IServiceInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            var options = new NotificationHubOptions();
            configuration.GetSection(nameof(NotificationHubOptions)).Bind(options);

            services.AddRabbitMqBroker(opt =>
            {
                opt.SimpleNotificationsQueue = options.SimpleNotificationsQueue;
                opt.PrivateNotificationsQueue = options.PrivateNotificationsQueue;
                opt.ExchangeName = options.ExchangeName;
            });
        }
    }
}
