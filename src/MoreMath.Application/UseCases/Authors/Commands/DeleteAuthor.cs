using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Authors.Commands;

public record DeleteAuthorCommand(int Id) : IRequest<ResultWrap>;



public class  DeleteAuthorHandler(IUnitOfWork unitOfWork):
    AbstractHandler<DeleteAuthorCommand, ResultWrap>(unitOfWork)
{
    public override async Task<ResultWrap> Handle(DeleteAuthorCommand command, CancellationToken cancellationToken)
    {
        var author = await _unitOfWork.AuthorRepo.FindAsync(command.Id);

        if (author == null)
        {
            return ResultWrap.Failure(new Error("Author.NotFound", "Failed to delete author with given Id. Author wasn't found."));
        }

        _unitOfWork.AuthorRepo.Delete(author);

        await _unitOfWork.CommitAsync();
        return ResultWrap.Success();
    }
}
