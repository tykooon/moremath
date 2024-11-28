using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Users.Commands;

public record LinkUserToAuthorCommand(
    int Id,
    int AuthorId) : IRequest<ResultWrap>;

public class LinkUserToAuthorHandler(IAppDbContext context) :
    AbstractHandler<LinkUserToAuthorCommand, ResultWrap>(context)
{
    public override async Task<ResultWrap> Handle(LinkUserToAuthorCommand command, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(command.Id);

        if (user == null)
        {
            return ResultWrap.Failure(new Error("User.NotFound", "Failed to update User with given Id. User wasn't found."));
        }

        var author = await _context.Authors.FindAsync(command.AuthorId);

        if (author == null)
        {
            return ResultWrap.Failure(new Error("Author.NotFound", "Failed to link User to author with given Id. Author wasn't found."));
        }

        user.Author = author;
        user.UpdateTimeMark();
        _context.Users.Update(user);

        await _context.SaveChangesAsync(cancellationToken);
        return ResultWrap.Success();
    }
}
