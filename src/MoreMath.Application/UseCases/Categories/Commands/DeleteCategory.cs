using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Categories.Commands;

public record DeleteCategoryCommand(int Id) : IRequest<ResultWrap>;

public class DeleteCategoryHandler(IUnitOfWork unitOfWork) :
    AbstractHandler<DeleteCategoryCommand, ResultWrap>(unitOfWork)
{
    public override async Task<ResultWrap> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.CategoryRepo.FindAsync(command.Id);
        if (category == null)
        {
            return ResultWrap.Failure(new Error("Category.NotFound", "Category with provided Id doesn't exist."));
        }

        _unitOfWork.CategoryRepo.Delete(category);
        await _unitOfWork.CommitAsync();

        return ResultWrap.Success();
    }
}
