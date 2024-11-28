using MediatR;
using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.HebWords.Commands;

public record DeleteTagFromHebWordCommand(
    int HebWordId,
    int TagId): IRequest<ResultWrap>;

public class DeleteTagFromHebWordHandler(IAppDbContext context):
    AbstractHandler<DeleteTagFromHebWordCommand, ResultWrap>(context)
{
    public override async Task<ResultWrap> Handle(DeleteTagFromHebWordCommand command, CancellationToken cancellationToken)
    {
        var hebWord = await _context.HebWords.Include(h => h.Tags).FirstOrDefaultAsync(hw => hw.Id.Equals(command.HebWordId));

        if(hebWord == null)
        {
            return ResultWrap.Failure(new Error("HebWord.NotFound", "Failed to get HebWord with given Id."));
        }

        var tag = await _context.Tags.FindAsync(command.TagId);

        if (tag == null)
        {
            return ResultWrap.Failure(new Error("Tag.NotFound", "Failed to get Tag with provided Id."));
        }

        hebWord.Tags.Remove(tag);

        await _context.SaveChangesAsync(cancellationToken);

        return ResultWrap.Success();
    }
}
