using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Authors.Commands;

public record UpdateAuthorCommand(
    int Id,
    string? FirstName,
    string? LastName,
    string? AvatarUri,
    string? Info,
    string? ShortBio) : IRequest<ResultWrap>;



public class  UpdateAuthorHandler(IUnitOfWork unitOfWork):
    AbstractHandler<UpdateAuthorCommand, ResultWrap>(unitOfWork)
{
    public override async Task<ResultWrap> Handle(UpdateAuthorCommand command, CancellationToken cancellationToken)
    {
        var author = await _unitOfWork.AuthorRepo.FindAsync(command.Id);

        if (author == null)
        {
            return ResultWrap.Failure([new Error("Author.NotFound", "Failed to update author with given Id. Author wasn't found.")]);
        }

        author.FirstName = command.FirstName ?? author.FirstName;
        author.LastName = command.LastName ?? author.LastName;
        author.Avatar = command.AvatarUri != null ? new Uri(command.AvatarUri) : author.Avatar;
        author.Info = command.Info ?? author.Info;
        author.ShortBio = command.ShortBio ?? author.ShortBio;

        _unitOfWork.AuthorRepo.Update(author);

        await _unitOfWork.CommitAsync();
        return ResultWrap.Success();
    }
}
