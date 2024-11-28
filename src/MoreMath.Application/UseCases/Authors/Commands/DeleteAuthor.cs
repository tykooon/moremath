using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Authors.Commands;

public record DeleteAuthorCommand(int Id) : IRequest<ResultWrap>;



public class  DeleteAuthorHandler(IAppDbContext context):
    AbstractHandler<DeleteAuthorCommand, ResultWrap>(context)
{
    public override async Task<ResultWrap> Handle(DeleteAuthorCommand command, CancellationToken cancellationToken)
    {
        var author = await _context.Authors.FindAsync(command.Id);

        if (author == null)
        {
            return ResultWrap.Failure(new Error("Author.NotFound", "Failed to delete author with given Id. Author wasn't found."));
        }

        _context.Authors.Remove(author);

        await _context.SaveChangesAsync(cancellationToken);
        return ResultWrap.Success();
    }
}
