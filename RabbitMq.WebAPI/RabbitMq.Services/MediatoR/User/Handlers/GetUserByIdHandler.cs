using MediatR;
using RabbitMq.Common.DTOs;
using RabbitMq.Services.Abstract;
using RabbitMq.Services.MediatoR.Requests;
using Serilog;

namespace RabbitMq.Services.MediatoR.User.Handlers
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdRequest, UserDto>
    {
        private readonly IUserService _service;
        private readonly ILogger _logger;
        public GetUserByIdHandler(IUserService service, ILogger logger)
        {
            _service = service;
            _logger = logger;
        }

        public Task<UserDto> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
        {
            _logger.Information("Request {requestName} is handling in {requestHandler} at {datetime}",
                typeof(GetUserByIdRequest).Name,
                typeof(GetUserByIdHandler).Name,
                DateTime.UtcNow);

            return _service.GetUserById(request.UserId, cancellationToken);
        }
    }
}
