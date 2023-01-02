using Newtonsoft.Json;
using RabbitMq.Common.Exceptions;
using Serilog.Events;
using System.Net;
using System.Text;
using ILogger = Serilog.ILogger;

namespace RabbitMq.WebAPI.Middlewares
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public ExceptionHandler(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch(Exception ex)
            {
                var status = ex switch
                {
                    UnreachableException => HttpStatusCode.Conflict,
                    ServiceImplementationException => HttpStatusCode.NotImplemented,
                    ValidationException => HttpStatusCode.BadRequest,
                    NotFoundException => HttpStatusCode.NotFound,
                    _ => HttpStatusCode.InternalServerError,
                };

                await Handle(ex, status, context);
            }
        }

        private async Task Handle(Exception ex, HttpStatusCode statusCode, HttpContext context)
        {
            if (_logger.IsEnabled(LogEventLevel.Error))
                _logger.Error(
                    "Error: {errorMessage}, Status Code: {statusCode}", 
                    ex.Message, statusCode);

            if ((statusCode == HttpStatusCode.InternalServerError ||
                statusCode == HttpStatusCode.Conflict) &&
                _logger.IsEnabled(LogEventLevel.Fatal))
            {
                var errorMessage = BuildCriticalLog(ex);
                _logger.Fatal("{errorMessage}", errorMessage);
            }

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(
                JsonConvert.SerializeObject(new { Error = ex.Message }));
        }

        private static string BuildCriticalLog(Exception? ex)
        {
            var sb = new StringBuilder();

            while(ex != null)
            {
                sb.AppendLine(ex.Message);
                sb.AppendLine(ex.StackTrace);
                ex = ex.InnerException;
            }

            return sb.ToString();
        }
    }
}
