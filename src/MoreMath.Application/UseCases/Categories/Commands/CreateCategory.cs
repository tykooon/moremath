using MediatR;
using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Core.Entities;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Categories.Commands;

public record CreateCategoryCommand(string CategoryName, string Description) : IRequest<ResultWrap<int>>;

public class CreateCategoryHandler(IAppDbContext context) :
    AbstractHandler<CreateCategoryCommand, ResultWrap<int>>(context)
{
    public override async Task<ResultWrap<int>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryName == command.CategoryName, cancellationToken);

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

        await _context.Categories.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return category.Id == 0
            ? ResultWrap.Failure(new Error("Category.CreateError", "Error while processing category creation"))
            : ResultWrap<int>.Success(category.Id);
    }
}
