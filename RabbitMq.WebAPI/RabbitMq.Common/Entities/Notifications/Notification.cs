namespace RabbitMq.Common.Entities.Notifications
{
    public abstract class Notification
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int RecieverId { get; set; }
        public string RecieverConnectionId { get; set; } = null!;
        public string? Content { get; set; }
    }
}
