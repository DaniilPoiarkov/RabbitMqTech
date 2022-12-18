using RabbitMq.Common.DTOs.NotificationsDto;
using RabbitMq.Common.Entities.Notifications;

namespace RabbitMq.Services.Abstract
{
    public interface IQueueService
    {
        Task SendValue<TNotification>(TNotification value, CancellationToken cancellationToken = default)
            where TNotification : Notification;

        Task<List<NotificationDto>> GetAllNotifications(int userId, CancellationToken cancellationToken = default);
    }
}
