using MediatR;
using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Articles.Commands;

public record DeleteTagFromArticleCommand(
    int ArticleId,
    int TagId): IRequest<ResultWrap>;

public class DeleteTagFromArticleHandler(IAppDbContext context):
    AbstractHandler<DeleteTagFromArticleCommand, ResultWrap>(context)
{
    public override async Task<ResultWrap> Handle(DeleteTagFromArticleCommand command, CancellationToken cancellationToken)
    {
        var article = await _context.Articles.Include(a => a.Tags).FirstOrDefaultAsync(a => a.Id == command.ArticleId, cancellationToken);

        if(article == null)
        {
            return ResultWrap.Failure(new Error("Article.NotFound", "Failed to get article with given Id."));
        }

        var Tag = await _context.Tags.FindAsync(command.TagId);

        if (Tag == null)
        {
            return ResultWrap.Failure(new Error("Tag.NotFound", "Failed to get Tag with provided Id."));
        }

        // TODO: Send or not 404 if article has no tag with given id ??

        article.Tags.Remove(Tag);
        article.UpdateTimeMark();

        await _context.SaveChangesAsync(cancellationToken);

        return ResultWrap.Success();
    }
}
