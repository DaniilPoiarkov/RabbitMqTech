using Microsoft.Extensions.DependencyInjection;
using RabbitMq.Broker.Hubs;
using RabbitMq.Broker.Hubs.SubscriptionHelper;
using RabbitMq.Broker.Interfaces;
using RabbitMq.Broker.Models.Options;
using RabbitMq.Broker.QueueServices;
using RabbitMq.Broker.Services;
using RabbitMq.Common.Exceptions;
using RabbitMQ.Client;

namespace RabbitMq.Broker.DIRegistration
{
    public static class RabbitMqDIRegistration
    {
        public static IServiceCollection AddRabbitMqBroker(this IServiceCollection services, Action<NotificationHubOptions> options)
        {
            services
                .Configure(options)
                .AddTransient<IProducerScopeFactory, ProducerScopeFactory>()
                .AddTransient<IConsumerScopeFactory, ConsumerScopeFactory>()
                .AddTransient<IConnectionFactory, ConnectionFactory>()

                .AddTransient<PrivateNotificationService>()
                .AddTransient<SimpleNotificationService>()

                .AddSingleton<ISubscriptionManager, SubscriptionManager>()
                .AddSingleton(sp =>
                {
                    var manager = sp.GetService<ISubscriptionManager>();
                    var consumerFactory = sp.GetService<IConsumerScopeFactory>();

                    if (manager == null ||
                        consumerFactory == null)
                        throw new ServiceImplementationException(
                            "Cannot create instance of notification hub, some dependencies can not be resolved");

                    var hubBuilder = new NotificationHubOptions();
                    options.Invoke(hubBuilder);

                    var privateQueueConsumer = consumerFactory.Connect(new()
                    {
                        ExchangeName = hubBuilder.ExchangeName,
                        ExchangeType = ExchangeType.Direct,
                        QueueName = hubBuilder.PrivateNotificationsQueue,
                        RoutingKey = hubBuilder.PrivateNotificationsQueue
                    });

                    var simpleQueueConsumer = consumerFactory.Connect(new()
                    {
                        ExchangeName = hubBuilder.ExchangeName,
                        ExchangeType = ExchangeType.Direct,
                        QueueName = hubBuilder.SimpleNotificationsQueue,
                        RoutingKey = hubBuilder.SimpleNotificationsQueue
                    });

                    return new NotificationHub(
                        privateQueueConsumer, 
                        simpleQueueConsumer, 
                        manager);
                });

            return services;
        }
    }
}
