using MediatR;
using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts;
using MoreMath.Application.Contracts.Services;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.HebWords.Commands;

public record AddTagsToHebWordCommand(
    int HebWordId,
    int[]? TagsId,
    string[]? TagNames): IRequest<ResultWrap>;

public class AddTagsToHebWordHandler(IAppDbContext context):
    AbstractHandler<AddTagsToHebWordCommand, ResultWrap>(context)
{
    public override async Task<ResultWrap> Handle(AddTagsToHebWordCommand command, CancellationToken cancellationToken)
    {
        var hebWord = await _context.HebWords.Include(h => h.Tags).FirstOrDefaultAsync(hw => hw.Id == command.HebWordId, cancellationToken);

        if (hebWord == null)
        {
            return ResultWrap.Failure(new Error("HebWord.NotFound", "Failed to get HebWord with given Id."));
        }

        var tagsById = command.TagsId?.Length > 0
            ? await _context.Tags.Where(t => command.TagsId.Contains(t.Id)).ToListAsync(cancellationToken)
            : [];

        var tagsByName = command.TagNames?.Length > 0
            ? await _context.Tags.Where(t => command.TagNames.Contains(t.TagName)).ToListAsync(cancellationToken)
            : [];

        tagsById.AddRange(tagsByName);

        if (tagsById.Count == 0)
        {
            return ResultWrap.Failure(new Error("Tags.NotFound", "Failed to get Tags with provided data."));
        }


        foreach (var tag in tagsById)
        {
            hebWord.Tags.Add(tag);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return ResultWrap.Success();
    }
}
