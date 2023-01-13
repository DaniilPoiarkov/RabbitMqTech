using MediatR;

namespace RabbitMq.Services.MediatoR.User.Requests
{
    public class SetConnectionIdRequest : IRequest
    {
        public int UserId { get; init; }
        public string ConnectionId { get; init; } = null!;
    }
}
