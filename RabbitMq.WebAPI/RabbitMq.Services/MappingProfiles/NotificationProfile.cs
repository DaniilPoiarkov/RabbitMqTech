using AutoMapper;
using RabbitMq.Common.DTOs.NotificationsDto;
using RabbitMq.Common.Entities.Notifications;

namespace RabbitMq.Services.MappingProfiles
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationDto>().ReverseMap();
            CreateMap<SimpleNotification, SimpleNotificationDto>().ReverseMap();
            CreateMap<PrivateNotification, PrivateNotificationDto>().ReverseMap();
            CreateMap<PublicNotification, PublicNotificationDto>().ReverseMap();
        }
    }
}
