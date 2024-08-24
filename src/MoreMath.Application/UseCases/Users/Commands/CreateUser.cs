using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;
using MediatR;
using MoreMath.Core.Entities;

namespace MoreMath.Application.UseCases.Users.Commands;

public record CreateUserCommand(
    string Username,
    bool IsActive) : IRequest<ResultWrap<int>>;


public class CreateUserHandler(IUnitOfWork unitOfWork) :
    AbstractHandler<CreateUserCommand, ResultWrap<int>>(unitOfWork)
{
    public override async Task<ResultWrap<int>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        User User = new()
        {
            Username = command.Username,
            IsActive = command.IsActive
        };

        await _unitOfWork.UserRepo.AddAsync(User);
        await _unitOfWork.CommitAsync();

        return User.Id == 0
            ? ResultWrap.Failure(new Error("User.Create", "User was not created"))
            : ResultWrap<int>.Success(User.Id);
    }
}

