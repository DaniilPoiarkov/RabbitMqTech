using MediatR;

namespace RabbitMq.Services.MediatoR.User.Requests
{
    public class UpdateAvatarUrlRequest : IRequest
    {
        public int UserId { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
