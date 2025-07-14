using Microsoft.AspNetCore.Http;
using SharedKernel;

namespace Web.Api.Infrastructure
{
    public static class CustomResults
    {
        public static IResult Problem(Result result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException();
            }

            return Results.Problem(
                title: GetTitle(result.Error),
                detail: GetDetail(result.Error),
                type: GetType(result.Error.Type),
                statusCode: GetStatusCode(result.Error.Type),
                extensions: GetError(result)
                );
        }

        private static Dictionary<string, object?>? GetError(Result result)
        {
            if(result.Error is not ValidationError validationError)
            {
                return null;
            }
            return new Dictionary<string, object?>
            {
              { "errors", validationError.Errors }
            };
        }

           static int? GetStatusCode(ErrorType errorType) =>
            errorType switch
            {
                ErrorType.Problem or ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Confilct => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };
           static string? GetType(ErrorType errorType) =>
            errorType switch
             {
                ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                ErrorType.Problem => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                ErrorType.Confilct => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
               _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
             };
                    

           static string? GetDetail(Error error) =>
            error.Type switch
            {
                ErrorType.Validation => error.Description,
                ErrorType.Problem => error.Description,
                ErrorType.NotFound => error.Description,
                ErrorType.Confilct => error.Description,
                _ => "An unexpected error occurred"
            };
      
        static string GetTitle(Error error) =>
            error.Type switch
            {
                ErrorType.Validation => error.Code,
                ErrorType.Problem => error.Code,
                ErrorType.NotFound => error.Code,
                ErrorType.Confilct => error.Code,
                _ => "Server failure"
            };
        
    }
}
