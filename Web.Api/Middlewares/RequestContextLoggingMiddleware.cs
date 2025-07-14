

using Microsoft.Extensions.Primitives;
using System.Diagnostics;
using System.Security.Claims;

namespace Web.Api.Middlewares
{
    public class RequestContextLoggingMiddleware(RequestDelegate next, ILogger<RequestContextLoggingMiddleware> logger)
    {
        private const string CorrelationIdHeaderName = "correlationId";
        public Task InvokeAsync(HttpContext context)
        {
            var data = new Dictionary<string, object>
            {
                ["correlationId"] = GetCorrelationId(context)
            };

            string? userId = GetUserID(context);
            if(userId is not null)
            {
                Activity.Current?.SetTag("user.id", userId);

                data["UserId"] = userId;
            }
            using (logger.BeginScope(data))
            {
                return next.Invoke(context);
            };
           
        }

        private static string? GetUserID(HttpContext context)
        {
            return context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private static string GetCorrelationId(HttpContext context)
        {
            context.Request.Headers.TryGetValue(
                                    CorrelationIdHeaderName,
                                    out StringValues correlationId);
            return correlationId.FirstOrDefault() ?? context.TraceIdentifier; 

        }
    }
}
