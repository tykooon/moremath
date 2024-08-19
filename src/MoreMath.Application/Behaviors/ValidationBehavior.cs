using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using MoreMath.Shared.Result;

namespace MoreMath.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : 
    IPipelineBehavior<TRequest, TResponse> where TRequest: IBaseRequest where TResponse: IResult
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = [];
    private readonly ILogger _logger;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, ILoggerFactory logger)
    {
        _validators = validators;
        _logger = logger.CreateLogger("ValidationLog");
}

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if(!_validators.Any())
        {
            return await next();
        }

        _logger.LogInformation("Validation of {Type} instance started", typeof(TRequest).Name);

        var context = new ValidationContext<TRequest>(request);

        var validationFailures = await Task.WhenAll(
            _validators.Select(validator => validator.ValidateAsync(context)));

        var errors = validationFailures
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .Select(validationFailure => Error.Validation(
                typeof(TRequest).Name,
                validationFailure.PropertyName,
                validationFailure.ErrorMessage))
            .ToArray();

        if (errors.Any())
        {
            if (typeof(TResponse) == typeof(Result))
            {
                var failedResponse = typeof(Result)
                .GetMethod(nameof(Result.Failure))!
                .Invoke(null, new object[] { errors })!; // creating object[] is essential to avoid wrong array interpretation

                _logger.LogInformation("Validation of {Type} instance failed", typeof(TRequest).Name);
                return (TResponse)failedResponse;
            }

            var resultType = typeof(TResponse).GenericTypeArguments[0];
            var validationFailedResponse = typeof(Result<>)
                .MakeGenericType(resultType)
                .GetMethod(nameof(Result.Failure))!
                .Invoke(null, new object[] { errors })!;

            _logger.LogInformation("Validation of {Type} instance failed", typeof(TRequest).Name);
            return (TResponse)validationFailedResponse;
        }

        _logger.LogInformation("Validation of {Type} instance succeeded", typeof(TRequest).Name);
        var response = await next();

        return response;
    }
}
