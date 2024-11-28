using MediatR;
using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Articles.Commands;

public record AddTagsToArticleCommand(
    int ArticleId,
    int[]? TagsId,
    string[]? TagNames): IRequest<ResultWrap>;

public class AddTagsToArticleHandler(IAppDbContext context):
    AbstractHandler<AddTagsToArticleCommand, ResultWrap>(context)
{
    public override async Task<ResultWrap> Handle(AddTagsToArticleCommand command, CancellationToken cancellationToken)
    {
        var article = await _context.Articles.Include(a => a.Tags).FirstOrDefaultAsync(a => a.Id ==  command.ArticleId, cancellationToken);

        if(article == null)
        {
            return ResultWrap.Failure(new Error("Article.NotFound", "Failed to get article with given Id."));
        }

        var tagsById = command.TagsId?.Length > 0
            ? await _context.Tags.Where(t =>command.TagsId.Contains(t.Id)).ToListAsync(cancellationToken)
            : [];

        var tagsByName = command.TagNames?.Length > 0
            ? await _context.Tags.Where(t => command.TagNames.Contains(t.TagName)).ToListAsync(cancellationToken)
            : [];

        tagsById.AddRange(tagsByName);

        if (tagsById.Count == 0)
        {
            return ResultWrap.Failure(new Error("Tags.NotFound", "Failed to get Tags with provided data."));
        }

        foreach(var tag in tagsById)
        {
            article.Tags.Add(tag);
        }

        article.UpdateTimeMark();

        await _context.SaveChangesAsync(cancellationToken);

        return ResultWrap.Success();
    }
}
