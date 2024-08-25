using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Users.Commands;

public record LinkUserToAuthorCommand(
    int Id,
    int AuthorId) : IRequest<ResultWrap>;

public class LinkUserToAuthorHandler(IUnitOfWork unitOfWork) :
    AbstractHandler<LinkUserToAuthorCommand, ResultWrap>(unitOfWork)
{
    public override async Task<ResultWrap> Handle(LinkUserToAuthorCommand command, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepo.FindAsync(command.Id);

        if (user == null)
        {
            return ResultWrap.Failure(new Error("User.NotFound", "Failed to update User with given Id. User wasn't found."));
        }

        var author = await _unitOfWork.AuthorRepo.FindAsync(command.AuthorId);

        if (author == null)
        {
            return ResultWrap.Failure(new Error("Author.NotFound", "Failed to link User to author with given Id. Author wasn't found."));
        }

        user.Author = author;
        _unitOfWork.UserRepo.Update(user);

        await _unitOfWork.CommitAsync();
        return ResultWrap.Success();
    }
}
