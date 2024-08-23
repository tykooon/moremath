using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Core.Entities;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Categories.Commands;

public record CreateCategoryCommand(string CategoryName, string Description) : IRequest<ResultWrap<int>>;

public class CreateCategoryHandler(IUnitOfWork unitOfWork) :
    AbstractHandler<CreateCategoryCommand, ResultWrap<int>>(unitOfWork)
{
    public override async Task<ResultWrap<int>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = (await _unitOfWork.CategoryRepo.GetFilteredAsync(x => x.CategoryName == command.CategoryName)).FirstOrDefault();
        if(category != null)
        {
            return ResultWrap.Failure(Error.Validation(
                typeof(CreateCategoryCommand).Name,
                nameof(command.CategoryName),
                "Category with provided categoryName already exists."));
        }

        category = new Category()
        {
            CategoryName = command.CategoryName,
            Description = command.Description
        };

        await _unitOfWork.CategoryRepo.AddAsync(category);
        await _unitOfWork.CommitAsync();
        return category.Id == 0
            ? ResultWrap.Failure(new Error("Category.CreateError", "Error while processing category creation"))
            : ResultWrap<int>.Success(category.Id);
    }
}
