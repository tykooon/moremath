using FluentValidation;
using MoreMath.Application.UseCases.Authors.Commands;

namespace MoreMath.Application.UseCases.Validators;

public class UpdateAuthorRequestValidator : AbstractValidator<UpdateAuthorCommand>
{
    public UpdateAuthorRequestValidator()
    {
        RuleFor(x => x.Id).NotEqual(0).WithMessage("Author Id should be peovided while updating. Id can not be equal to 0.");
        RuleFor(x => x)
            .Must(x => x.FirstName != null || x.LastName != null || x.AvatarUri != null || x.Info != null || x.ShortBio != null)
            .WithMessage("Aauthor update request should contain at least one property with new value");
        RuleFor(x => x.FirstName).Must(p => p is null || !string.IsNullOrWhiteSpace(p));
        RuleFor(x => x.AvatarUri).Must(p => p is null || Uri.IsWellFormedUriString(p, UriKind.RelativeOrAbsolute));
    }
}
