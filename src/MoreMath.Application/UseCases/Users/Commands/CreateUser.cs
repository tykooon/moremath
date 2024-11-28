using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;
using MediatR;
using MoreMath.Core.Entities;

namespace MoreMath.Application.UseCases.Users.Commands;

public record CreateUserCommand(
    string Username,
    bool IsActive) : IRequest<ResultWrap<int>>;


public class CreateUserHandler(IAppDbContext context) :
    AbstractHandler<CreateUserCommand, ResultWrap<int>>(context)
{
    public override async Task<ResultWrap<int>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        User user = new()
        {
            Username = command.Username,
            IsActive = command.IsActive
        };

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return user.Id == 0
            ? ResultWrap.Failure(new Error("User.Create", "User was not created"))
            : ResultWrap<int>.Success(user.Id);
    }
}

