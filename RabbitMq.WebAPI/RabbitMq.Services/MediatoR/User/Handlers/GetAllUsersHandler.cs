using MediatR;
using RabbitMq.Common.DTOs;
using RabbitMq.Services.Abstract;
using RabbitMq.Services.MediatoR.User.Requests;
using Serilog;

namespace RabbitMq.Services.MediatoR.User.Handlers
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersRequest, List<UserDto>>
    {
        private readonly IUserService _service;
        private readonly ILogger _logger;

        public GetAllUsersHandler(IUserService service, ILogger logger)
        {
            _service = service;
            _logger = logger;
        }

        public Task<List<UserDto>> Handle(GetAllUsersRequest request, CancellationToken cancellationToken)
        {
            _logger.Information("Request {requestName} is handling in {requestHandler} at {datetime}",
                typeof(GetAllUsersRequest).Name,
                typeof(GetAllUsersHandler).Name,
                DateTime.UtcNow);

            return _service.GetAllUsers(cancellationToken);
        }
    }
}
