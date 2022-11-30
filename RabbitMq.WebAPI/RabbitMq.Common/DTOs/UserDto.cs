
namespace RabbitMq.Common.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string ConnectionId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public List<NotificationsDto.NotificationDto> Notifications { get; set; } = new();
    }
}
