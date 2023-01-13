using MediatR;
using RabbitMq.Common.DTOs;

namespace RabbitMq.Services.MediatoR.Requests
{
    public class GetUserByIdRequest : IRequest<UserDto>
    {
        public int UserId { get; init; }
    }
}
