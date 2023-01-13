using MediatR;
using RabbitMq.Common.DTOs;

namespace RabbitMq.Services.MediatoR.User.Requests
{
    public class GetAllUsersRequest : IRequest<List<UserDto>>
    {
    }
}
