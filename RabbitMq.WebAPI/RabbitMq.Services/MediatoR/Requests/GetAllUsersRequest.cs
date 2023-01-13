using MediatR;
using RabbitMq.Common.DTOs;

namespace RabbitMq.Services.MediatoR.Requests
{
    public class GetAllUsersRequest : IRequest<List<UserDto>>
    {
    }
}
