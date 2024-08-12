using Microsoft.AspNetCore.Diagnostics;
using MoreMath.Api.Models;
using System.Net;

namespace MoreMath.Api.Exceptions;

public class GlobalExceptionHandler(ILoggerFactory logger) : IExceptionHandler
{
    private readonly ILogger _logger = logger.CreateLogger("ExceptionLog");

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = "application/json";

        _logger.LogError($"Something went wrong: {exception.Message}");

        var message = exception switch
        {
            // TODO: Specify Global Exceptions
            AccessViolationException => "Access violation error",
            _ => "Internal Server Error"
        };

        await httpContext.Response.WriteAsync(
            new ErrorDetails()
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = message,
            }.ToString(),
            cancellationToken: cancellationToken);

        return true;
    }
}
