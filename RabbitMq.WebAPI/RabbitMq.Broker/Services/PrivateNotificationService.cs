using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMq.Broker.Interfaces;
using RabbitMq.Broker.Models.Options;
using RabbitMq.Common.DTOs.NotificationsDto;
using RabbitMq.Common.Entities.Notifications;
using RabbitMq.DAL;

namespace RabbitMq.Broker.Services
{
    public class PrivateNotificationService : NotificationService<PrivateNotification, PrivateNotificationDto>
    {
        public PrivateNotificationService(
            RabbitMqDb db, 
            IProducerScopeFactory producerFactory,
            IMapper mapper,
            IOptions<NotificationHubOptions> hubOptions) : 
            base(db, mapper, producerFactory, hubOptions)
        { }

        public override async Task<List<PrivateNotificationDto>> GetNotifications(int recieverId, CancellationToken cancellationToken = default)
        {
            return _mapper.Map<List<PrivateNotificationDto>>(
                await _db.PrivateNotifications
                    .Where(n => n.RecieverId == recieverId)
                    .Include(n => n.Sender)
                    .ToListAsync(cancellationToken));
        }
    }
}
