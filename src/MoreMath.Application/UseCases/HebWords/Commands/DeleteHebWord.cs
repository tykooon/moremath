using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.HebWords.Commands;

public record DeleteHebWordCommand(int Id) : IRequest<ResultWrap>;



public class  DeleteHebWordHandler(IAppDbContext context):
    AbstractHandler<DeleteHebWordCommand, ResultWrap>(context)
{
    public override async Task<ResultWrap> Handle(DeleteHebWordCommand command, CancellationToken cancellationToken)
    {
        var HebWord = await _context.HebWords.FindAsync(command.Id);

        if (HebWord == null)
        {
            return ResultWrap.Failure(new Error("HebWord.NotFound", "Failed to delete author with given Id. Author wasn't found."));
        }

        _context.HebWords.Remove(HebWord);

        await _context.SaveChangesAsync(cancellationToken);
        return ResultWrap.Success();
    }
}
