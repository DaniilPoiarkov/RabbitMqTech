using AutoMapper;
using Microsoft.Extensions.Options;
using RabbitMq.Broker.Interfaces;
using RabbitMq.Broker.Models.Options;
using RabbitMq.Common.DTOs.NotificationsDto;
using RabbitMq.Common.Entities.Notifications;
using RabbitMq.DAL;

namespace RabbitMq.Broker.Services
{
    public class SimpleNotificationService : NotificationService<SimpleNotification, SimpleNotificationDto>
    {
        public SimpleNotificationService(
            RabbitMqDb db, 
            IProducerScopeFactory producerFactory,
            IMapper mapper,
            IOptions<NotificationHubOptions> hubOptions) :
            base(db, mapper, producerFactory, hubOptions)
        { }
    }
}
