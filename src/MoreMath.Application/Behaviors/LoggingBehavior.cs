using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using MoreMath.Shared.Result;

namespace MoreMath.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseRequest where TResponse : IResultWrap
{
    private readonly ILogger _logger;

    public LoggingBehavior(IEnumerable<IValidator<TRequest>> validators, ILoggerFactory logger)
    {
        _logger = logger.CreateLogger("RequestLog");
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing request {Type} started", typeof(TRequest).Name);
        
        TResponse response = await next();

        if (response.IsSuccessfull)
        {
            _logger.LogInformation("Request {Type} completed successfully", typeof(TRequest).Name);
        }
        else
        {
            _logger.LogError("Request {Type} failed to complete", typeof(TRequest).Name);
        }

        return response;
    }
}
