using MediatR;
using ILogger = Serilog.ILogger;

namespace RabbitMq.WebAPI.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger;
        public LoggingBehavior(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.Information("Start handling {RequestName} at {DateTime}", 
                typeof(TRequest).Name,
                DateTime.UtcNow);

            try
            {
                return await next.Invoke();
            }
            catch (Exception ex)
            {
                _logger.Error("Exception: {ExceptionType} while handling {RequestName}, Message: {ExceptionMessage} at {DateTime}",
                    ex.GetType().Name,
                    typeof(TRequest).Name,
                    ex.Message,
                    DateTime.UtcNow);

                throw;
            }
        }
    }
}
