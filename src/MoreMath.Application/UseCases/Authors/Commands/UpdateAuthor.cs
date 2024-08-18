using FluentValidation;
using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Authors.Commands;

public record UpdateAuthorCommand(
    int Id,
    string? FirstName,
    string? LastName,
    string? AvatarUri,
    string? Info,
    string? ShortBio) : IRequest<Result>;

public class  UpdateAuthorHandler: IRequestHandler<UpdateAuthorCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    IValidator<UpdateAuthorCommand> _validator;

    public UpdateAuthorHandler(IUnitOfWork unitOfWork, IValidator<UpdateAuthorCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result> Handle(UpdateAuthorCommand command, CancellationToken cancellationToken)
    {
        //var res = await _validator.ValidateAsync(command, cancellationToken);
        //if (!res.IsValid)
        //{
        //    var result = Result.Failure(new Error("Author.Create.Validation"));
        //    foreach (var err in res.Errors)
        //    {
        //        result.AppendError(new Error(err.ErrorCode, err.ErrorMessage));
        //    }
        //    return result;
        //}

        var author = await _unitOfWork.AuthorRepo.FindAsync(command.Id);

        if (author == null)
        {
            return Result.Failure(new("Author.NotFound", "Failed to update author with given Id. Author wasn't found."));
        }

        author.FirstName = command.FirstName ?? author.FirstName;
        author.LastName = command.LastName ?? author.LastName;
        author.Avatar = command.AvatarUri != null ? new Uri(command.AvatarUri) : author.Avatar;
        author.Info = command.Info ?? author.Info;
        author.ShortBio = command.ShortBio ?? author.ShortBio;

        _unitOfWork.AuthorRepo.Update(author);

        await _unitOfWork.CommitAsync();
        return Result.Success();
    }
}
