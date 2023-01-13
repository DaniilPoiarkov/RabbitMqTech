using MediatR;
using RabbitMq.Common.DTOs;

namespace RabbitMq.Services.MediatoR.User.Requests
{
    public class GetUserByIdRequest : IRequest<UserDto>
    {
        public int UserId { get; init; }
    }
}
