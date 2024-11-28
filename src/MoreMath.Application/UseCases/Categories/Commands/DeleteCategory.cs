using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Categories.Commands;

public record DeleteCategoryCommand(int Id) : IRequest<ResultWrap>;

public class DeleteCategoryHandler(IAppDbContext context) :
    AbstractHandler<DeleteCategoryCommand, ResultWrap>(context)
{
    public override async Task<ResultWrap> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FindAsync(command.Id);
        if (category == null)
        {
            return ResultWrap.Failure(new Error("Category.NotFound", "Category with provided Id doesn't exist."));
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);

        return ResultWrap.Success();
    }
}
