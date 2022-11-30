﻿namespace RabbitMq.Common.DTOs.NotificationsDto
{
    public abstract class NotificationDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int RecieverId { get; set; }
        public UserDto Reciever { get; set; } = null!;
        public string RecieverConnectionId { get; set; } = null!;
        public string? Content { get; set; }
        public override string ToString() => $"Id: {Id}, Created: {CreatedAt}";
    }
}
