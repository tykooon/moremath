using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using MoreMath.Shared.Result;

namespace MoreMath.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : 
    IPipelineBehavior<TRequest, TResponse> where TRequest: IBaseRequest where TResponse: IResultWrap
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = [];

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, ILoggerFactory logger)
    {
        _validators = validators;
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
            if (typeof(TResponse) == typeof(ResultWrap))
            {
                var failedResponse = typeof(ResultWrap)
                .GetMethod(nameof(ResultWrap.Failure))!
                .Invoke(null, new object[] { errors })!; // creating object[] is essential to avoid wrong array interpretation
                return (TResponse)failedResponse;
            }

            var resultType = typeof(TResponse).GenericTypeArguments[0];
            var validationFailedResponse = typeof(ResultWrap<>)
                .MakeGenericType(resultType)
                .GetMethod(nameof(ResultWrap.Failure))!
                .Invoke(null, new object[] { errors })!;

            return (TResponse)validationFailedResponse;
        }

        var response = await next();

        return response;
    }
}
