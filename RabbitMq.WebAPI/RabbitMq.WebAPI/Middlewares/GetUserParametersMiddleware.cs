using RabbitMq.Common.Parameters;
using RabbitMq.Identity.Statics;
using System.Security.Claims;

namespace RabbitMq.WebAPI.Middlewares
{
    public class GetUserParametersMiddleware
    {
        private readonly RequestDelegate _next;

        public GetUserParametersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, UserParameters userParameters)
        {
            if (int.TryParse(context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out var userId))
                userParameters.UserId = userId;
            if (context.User.FindFirst(ClaimTypes.Email)?.Value != null)
                userParameters.Email = context.User.FindFirst(ClaimTypes.Email)?.Value;

            await _next.Invoke(context);
        }
    }
}
