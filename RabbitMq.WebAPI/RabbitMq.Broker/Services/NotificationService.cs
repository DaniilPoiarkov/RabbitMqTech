using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMq.Broker.Interfaces;
using RabbitMq.Broker.Models.Options;
using RabbitMq.Common.DTOs.NotificationsDto;
using RabbitMq.Common.Entities;
using RabbitMq.Common.Entities.Notifications;
using RabbitMq.Common.Exceptions;
using RabbitMq.DAL;
using RabbitMQ.Client;

namespace RabbitMq.Broker.Services
{
    public abstract class NotificationService<TNotification, TDto>
        where TNotification : Notification
        where TDto : NotificationDto
    {
        protected readonly RabbitMqDb _db;
        protected readonly IMapper _mapper;
        protected readonly IProducerScopeFactory _factory;
        protected readonly NotificationHubOptions _hubOptions;
        public NotificationService(
            RabbitMqDb db,
            IMapper mapper,
            IProducerScopeFactory factory,
            IOptions<NotificationHubOptions> hubOptions)
        {
            _db = db;
            _mapper = mapper;
            _factory = factory;
            _hubOptions = hubOptions.Value;
        }

        public virtual Task DeleteNotification(int id, CancellationToken cancellationToken = default)
        {
            return _db.Set<TNotification>()
                .Where(n => n.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
        }

        public virtual async Task CreateAndSendNotification(TDto notification, CancellationToken cancellationToken = default)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == notification.RecieverId, cancellationToken);

            var entity = _mapper.Map<TNotification>(notification);

            if (user == null)
                throw new NotFoundException(nameof(User));

            entity.RecieverConnectionId = user.ConnectionId;

            await _db.Set<TNotification>().AddAsync(entity, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            using var producer = _factory.Open(new()
            {
                ExchangeName = _hubOptions.ExchangeName,
                ExchangeType = ExchangeType.Direct,
                QueueName = typeof(TNotification).Name,
                RoutingKey = typeof(TNotification).Name,
            });

            producer.Producer.Send(
                JsonConvert.SerializeObject(entity));
        }

        public virtual async Task<List<TDto>> GetNotifications(int recieverId, CancellationToken cancellationToken = default)
        {
            return _mapper.Map<List<TDto>>(
                await _db.Set<TNotification>()
                    .Where(n => n.RecieverId == recieverId)
                    .ToListAsync(cancellationToken));
        }
    }
}
