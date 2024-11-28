using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Users.Commands;

public record DeleteUserCommand(int Id) : IRequest<ResultWrap>;

public class DeleteUserHandler(IAppDbContext context) :
    AbstractHandler<DeleteUserCommand, ResultWrap>(context)
{
    public override async Task<ResultWrap> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var User = await _context.Users.FindAsync(command.Id);

        if (User == null)
        {
            return ResultWrap.Failure(new Error("User.NotFound", "Failed to delete User with given Id. User wasn't found."));
        }

        _context.Users.Remove(User);

        await _context.SaveChangesAsync(cancellationToken);
        return ResultWrap.Success();
    }
}
