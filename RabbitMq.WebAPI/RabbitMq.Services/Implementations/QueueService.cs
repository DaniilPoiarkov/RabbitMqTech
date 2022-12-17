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
using RabbitMq.Services.Abstract;
using RabbitMQ.Client;

namespace RabbitMq.Services.Implementations
{
    public class QueueService : IQueueService
    {
        private readonly IProducerScopeFactory _producerFactory;
        private readonly RabbitMqDb _db;
        private readonly IMapper _mapper;
        private readonly NotificationHubOptions _hubOptions;
        public QueueService(
            IProducerScopeFactory producerFactory,
            RabbitMqDb db, 
            IMapper mapper,
            IOptions<NotificationHubOptions> hubOptions)
        {
            _producerFactory = producerFactory;
            _db = db;
            _mapper = mapper;
            _hubOptions = hubOptions.Value;
        }

        public async Task SendValue<TNotification>(TNotification value, CancellationToken cancellationToken = default)
            where TNotification : Notification
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == value.RecieverId, cancellationToken);

            if (user == null)
                throw new NotFoundException(nameof(User));

            value.RecieverConnectionId = user.ConnectionId;

            await _db.Set<TNotification>().AddAsync(value, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            var producer = _producerFactory.Open(new()
            {
                ExchangeName = _hubOptions.ExchangeName,
                ExchangeType = ExchangeType.Direct,
                QueueName = typeof(TNotification).Name,
                RoutingKey = typeof(TNotification).Name,
            });

            producer.Producer.Send(
                JsonConvert.SerializeObject(value));
        }

        public async Task<List<NotificationDto>> GetAllNotifications(int userId, CancellationToken cancellationToken = default)
        {
            var privateNotifications = _mapper.Map<List<PrivateNotificationDto>>(
                await _db.PrivateNotifications.Where(n => n.RecieverId == userId).ToListAsync(cancellationToken));

            var simpleNotifications = _mapper.Map<List<SimpleNotificationDto>>(
                await _db.SimpleNotifications.Where(n => n.RecieverId == userId).ToListAsync(cancellationToken));

            var notifications = new List<NotificationDto>();

            notifications.AddRange(privateNotifications);
            notifications.AddRange(simpleNotifications);

            return notifications.OrderBy(n => DateTime.Today - n.CreatedAt).ToList();
        }
    }
}
