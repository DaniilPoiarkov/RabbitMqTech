using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMq.Broker.Models.Options;
using RabbitMq.Broker.QueueServices;
using RabbitMq.Broker.Services;
using RabbitMq.Common.DTOs.NotificationsDto;
using RabbitMq.Common.Entities.Notifications;
using RabbitMq.Common.Exceptions;
using RabbitMq.DAL;
using RabbitMq.Services.MappingProfiles;
using RabbitMQ.Client;

namespace RabbitMq.Services.Tests
{
    public class NotificationServiceTests
    {
        private readonly PrivateNotificationService _privateNotificationService;
        private readonly SimpleNotificationService _simpleNotificationService;

        public NotificationServiceTests()
        {
            var db = new RabbitMqDb(
                new DbContextOptionsBuilder<RabbitMqDb>()
                .UseInMemoryDatabase("TestDb:" + Guid.NewGuid())
                .Options);

            db.Users.Add(new()
            {
                Id = 1,
                Email = "test",
                Password = "test",
                Salt = "test",
                Username = "test",
                ConnectionId = "test"
            });

            db.Users.Add(new()
            {
                Id = 2,
                Email = "test",
                Password = "test",
                Salt = "test",
                Username = "test",
                ConnectionId = "test"
            });

            db.SaveChanges();

            var producerScopeFactory = new ProducerScopeFactory(
                new ConnectionFactory());

            var mapper = new MapperConfiguration(opt =>
            {
                opt.AddProfile<UserProfile>();
                opt.AddProfile<NotificationProfile>();
            }).CreateMapper();

            var hubOptions = Options.Create(new NotificationHubOptions()
            {
                ExchangeName = "test",
                SimpleNotificationsQueue = nameof(SimpleNotification),
                PrivateNotificationsQueue = nameof(PrivateNotification),
            });

            _privateNotificationService = new(db, producerScopeFactory, mapper, hubOptions);
            _simpleNotificationService = new(db, producerScopeFactory, mapper, hubOptions);
        }

        [Fact]
        public async Task CreateAndSendNotification_WhenValidData_ThenOneNotificationCreated()
        {
            await _privateNotificationService.CreateAndSendNotification(new PrivateNotificationDto()
            {
                Content = "test",
                CreatedAt = DateTime.UtcNow,
                RecieverId = 1,
                SenderId = 2,
            });

            var notifications = await _privateNotificationService.GetNotifications(1);

            Assert.Single(notifications);
            Assert.True(notifications.All(n => n.Content == "test"));
        }

        [Fact]
        public async Task CreateAndSendNotification_WhenInvalidRecieverId_ThenNotFoundExceptionThrows()
        {
            await Assert.ThrowsAsync<NotFoundException>(async () => await _simpleNotificationService.CreateAndSendNotification(new()
            {
                Id = 1,
                Content = "test",
                CreatedAt = DateTime.UtcNow,
                RecieverId = 3,
            }));
        }

    }
}
