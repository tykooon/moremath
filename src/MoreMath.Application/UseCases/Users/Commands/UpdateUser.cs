using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Users.Commands;

public record UpdateUserCommand(
    int Id,
    string? Username,
    bool? IsActive) : IRequest<ResultWrap>;

public class UpdateUserHandler(IAppDbContext context) :
    AbstractHandler<UpdateUserCommand, ResultWrap>(context)
{
    public override async Task<ResultWrap> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(command.Id);

        if (user == null)
        {
            return ResultWrap.Failure(new Error("User.NotFound", "Failed to update User with given Id. User wasn't found."));
        }

        user.Username = command.Username ?? user.Username;
        user.IsActive = command.IsActive ?? user.IsActive;

        _context.Users.Update(user);

        await _context.SaveChangesAsync(cancellationToken);
        return ResultWrap.Success();
    }
}
