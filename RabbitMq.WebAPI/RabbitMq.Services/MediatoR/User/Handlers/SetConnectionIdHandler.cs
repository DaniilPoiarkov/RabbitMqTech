using MediatR;
using RabbitMq.Services.Abstract;
using RabbitMq.Services.MediatoR.User.Requests;
using Serilog;

namespace RabbitMq.Services.MediatoR.User.Handlers
{
    internal class SetConnectionIdHandler : IRequestHandler<SetConnectionIdRequest, Unit>
    {
        private readonly IUserService _service;
        private readonly ILogger _logger;

        public SetConnectionIdHandler(IUserService service, ILogger logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<Unit> Handle(SetConnectionIdRequest request, CancellationToken cancellationToken)
        {
            _logger.Information("Request {requestName} is handling in {requestHandler} at {datetime}",
                typeof(SetConnectionIdRequest).Name,
                typeof(SetConnectionIdHandler).Name,
                DateTime.UtcNow);

            await _service.SetConnectionId(request.ConnectionId, request.UserId, cancellationToken);
            return Unit.Value;
        }
    }
}
