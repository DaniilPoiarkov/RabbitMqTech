
using MediatR;
using RabbitMq.Common.DTOs;

namespace RabbitMq.Services.MediatoR.Requests
{
    public class GetUserByEmailRequest : IRequest<UserDto>
    {
        public string UserEmail { get; init; } = null!;
    }
}
