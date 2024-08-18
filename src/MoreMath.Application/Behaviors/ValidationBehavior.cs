using FluentValidation;
using MediatR;
using MoreMath.Shared.Result;

namespace MoreMath.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : 
    IPipelineBehavior<TRequest, TResponse> where TRequest: IBaseRequest
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = [];

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationFailures = await Task.WhenAll(
            _validators.Select(validator => validator.ValidateAsync(context)));

        var errors = validationFailures
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .Select(validationFailure => new Error(
                $"Validation.{validationFailure.PropertyName}.{validationFailure.ErrorCode}",
                validationFailure.ErrorMessage))
            .ToList();

        if (errors.Any())
        {
            throw new Exception(errors.Select(err => err.Message).Aggregate((str, er) => str + " | " + er));
        }

        var response = await next();

        return response;

    }
}
