using FluentValidation;
using MoreMath.Application.UseCases.Authors.Commands;

namespace MoreMath.Application.UseCases.Validators;

public class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>
{
    public CreateAuthorCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name can not be empty");
        RuleFor(x => x.LastName).NotNull().WithMessage("Last name can not be null");
        RuleFor(x => x.AvatarUri).Matches("^(((http|https|ftp):\\/\\/)|\\/)?[^\\s\\/$.?#].[^\\s]*$").WithMessage("Provided URL for Author avatar is not valid.");
        RuleFor(x => x.Info).NotNull().WithMessage("Info about author can not be null");
        RuleFor(x => x.ShortBio).NotNull().WithMessage("ShortBio of author can not be null");
    }
}
