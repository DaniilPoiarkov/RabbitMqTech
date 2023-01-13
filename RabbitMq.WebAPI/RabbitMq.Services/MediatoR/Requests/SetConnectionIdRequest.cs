using MediatR;

namespace RabbitMq.Services.MediatoR.Requests
{
    public class SetConnectionIdRequest : IRequest
    {
        public int UserId { get; init; }
        public string ConnectionId { get; init; } = null!;
    }
}
