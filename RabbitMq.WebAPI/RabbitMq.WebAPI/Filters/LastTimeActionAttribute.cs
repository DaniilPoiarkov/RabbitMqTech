using Microsoft.AspNetCore.Mvc.Filters;

namespace RabbitMq.WebAPI.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class LastTimeActionAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.Response.Cookies.Append("LastAction", $"{DateTime.UtcNow.ToShortDateString()}");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Response.Headers.Append("LastAction", $"{DateTime.UtcNow.ToShortDateString()}");
        }
    }
}
