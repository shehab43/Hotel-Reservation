using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
        where TRequest : IRequest<TResponse>

    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
                                TRequest request,
                                RequestHandlerDelegate<TResponse> next, 
                                CancellationToken cancellationToken)
        {

            string requestName = typeof(TRequest).Name;
            string operationType = GetOperationType(requestName);
            var sw = Stopwatch.StartNew();

            _logger.LogInformation("Processing {OperationType} {Request}", operationType, requestName);

            try
            {
                var response = await next();

                if (response is Result result)
                {
                    if (result.IsSuccess)
                    {
                        _logger.LogInformation("Completed {OperationType} {Request}", operationType, requestName);
                    }
                    else
                    {
                        var data = new Dictionary<string, object>
                        {
                            ["Error"] = result.Error
                        };
                        using (_logger.BeginScope(data))
                        {
                            _logger.LogError("Completed {OperationType} {Request} with error", operationType, requestName);
                        }
                    }
                }
                else
                {
                    _logger.LogInformation("Completed {OperationType} {Request}", operationType, requestName);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing {OperationType} {Request}", operationType, requestName);
                throw;
            }
            finally
            {
                sw.Stop();
                _logger.LogInformation(
                    "Duration {OperationType} {RequestName}: {ElapsedMs}ms (RequestId={RequestId}, CorrelationId={CorrelationId})",
                    operationType, requestName, sw.ElapsedMilliseconds,
                    Activity.Current?.TraceId.ToString(), // relies on middleware/hosting
                    Activity.Current?.GetTagItem("correlation.id"));
            }
        }

        private static string GetOperationType(string requestName)
        {
            if (requestName.EndsWith("Command", StringComparison.OrdinalIgnoreCase))
                return "command";
            if (requestName.EndsWith("Query", StringComparison.OrdinalIgnoreCase))
                return "query";
            return "request";
        }
    }
    
}
