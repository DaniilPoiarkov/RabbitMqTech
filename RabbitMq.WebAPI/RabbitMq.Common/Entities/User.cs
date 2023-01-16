
using RabbitMq.Common.Entities.Notifications;

namespace RabbitMq.Common.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string ConnectionId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public List<Notification> Notifications { get; set; } = new();
    }
}
