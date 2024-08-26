using FluentValidation;
using MoreMath.Application.UseCases.Authors.Commands;

namespace MoreMath.Application.UseCases.Validators;

public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
{
    public UpdateAuthorCommandValidator()
    {
        RuleFor(x => x.Id).NotEqual(0).WithMessage("Author Id should be provided while updating. Id can not be equal to 0.");
        RuleFor(x => x).Must(x =>
            x.FirstName != null ||
            x.LastName != null ||
            x.AvatarUri != null ||
            x.Info != null ||
            x.ShortBio != null ||
            x.Phone != null ||
            x.Email != null ||
            x.WhatsApp != null ||
            x.Telegram != null ||
            x.Website != null ||
            x.Options != null).WithMessage("Author update request should contain at least one property with new value");
        RuleFor(x => x.FirstName).Must(p => p is null || !string.IsNullOrWhiteSpace(p));
        RuleFor(x => x.AvatarUri).Matches("^(((http|https|ftp):\\/\\/)|\\/)?[^\\s\\/$.?#].[^\\s]*$").WithMessage("Provided URL for Author avatar is not valid.");
    }
}
