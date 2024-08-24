using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Users.Commands;

public record DeleteUserCommand(int Id) : IRequest<ResultWrap>;

public class DeleteUserHandler(IUnitOfWork unitOfWork) :
    AbstractHandler<DeleteUserCommand, ResultWrap>(unitOfWork)
{
    public override async Task<ResultWrap> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var User = await _unitOfWork.UserRepo.FindAsync(command.Id);

        if (User == null)
        {
            return ResultWrap.Failure(new Error("User.NotFound", "Failed to delete User with given Id. User wasn't found."));
        }

        _unitOfWork.UserRepo.Delete(User);

        await _unitOfWork.CommitAsync();
        return ResultWrap.Success();
    }
}
