

using Microsoft.Extensions.Primitives;
using System.Diagnostics;
using System.Security.Claims;

namespace Web.Api.Middlewares
{
    public class RequestContextLoggingMiddleware
    {
        private const string CorrelationIdHeaderName = "correlationId";
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestContextLoggingMiddleware> _logger;

        public RequestContextLoggingMiddleware(RequestDelegate next, ILogger<RequestContextLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

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
            using (_logger.BeginScope(data))
            {
                return _next.Invoke(context);
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
