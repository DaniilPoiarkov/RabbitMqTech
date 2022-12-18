using RabbitMq.Common.Parameters;

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
            if (int.TryParse(context.User.FindFirst("id")?.Value, out var userId))
                userParameters.UserId = userId;
            if (context.User.FindFirst(
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value != null)
                userParameters.Email = context.User.FindFirst(
                    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;

            await _next.Invoke(context);
        }
    }
}
