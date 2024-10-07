using FluentValidation;
using MoreMath.Application.UseCases.Authors.Commands;

namespace MoreMath.Application.UseCases.Validators;

public class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>
{
    public CreateAuthorCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name can not be empty");
        RuleFor(x => x.LastName).NotNull().WithMessage("Last name can not be null");
        RuleFor(x => x.SlugName).NotNull().WithMessage("Short name (slug) for Author should be provided.");
        RuleFor(x => x.Info).NotNull().WithMessage("Info about author can not be null");
        RuleFor(x => x.ShortBio).NotNull().WithMessage("ShortBio of author can not be null");
    }
}
