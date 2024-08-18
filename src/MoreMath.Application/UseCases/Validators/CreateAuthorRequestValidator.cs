using FluentValidation;
using MoreMath.Application.UseCases.Authors.Commands;

namespace MoreMath.Application.UseCases.Validators;

public class CreateAuthorRequestValidator : AbstractValidator<CreateAuthorCommand>
{
    public CreateAuthorRequestValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name can not be empty");
        RuleFor(x => x.AvatarUri).Must(uri => Uri.TryCreate(uri, UriKind.RelativeOrAbsolute, out _)).WithMessage("Provided URL for Author avatar is not valid.");
    }
}
