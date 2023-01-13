using MediatR;
using RabbitMq.Common.DTOs;
using RabbitMq.Services.Abstract;
using RabbitMq.Services.MediatoR.Requests;
using Serilog;

namespace RabbitMq.Services.MediatoR.User.Handlers
{
    public class GetUserByEmailHandler : IRequestHandler<GetUserByEmailRequest, UserDto>
    {
        private readonly IUserService _service;
        private readonly ILogger _logger;

        public GetUserByEmailHandler(IUserService service, ILogger logger)
        {
            _service = service;
            _logger = logger;
        }

        public Task<UserDto> Handle(GetUserByEmailRequest request, CancellationToken cancellationToken)
        {
            _logger.Information("Request {requestName} is handling in {requestHandler} at {datetime}",
                typeof(GetUserByEmailRequest).Name,
                typeof(GetUserByEmailHandler).Name,
                DateTime.UtcNow);

            return _service.GetUserByEmail(request.UserEmail, cancellationToken);
        }
    }
}
