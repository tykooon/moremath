using MediatR;
using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Articles.Commands;

public record DeleteAuthorFromArticleCommand(
    int ArticleId,
    int AuthorId): IRequest<ResultWrap>;

public class DeleteAuthorFromArticleHandler(IAppDbContext context):
    AbstractHandler<DeleteAuthorFromArticleCommand, ResultWrap>(context)
{
    public override async Task<ResultWrap> Handle(DeleteAuthorFromArticleCommand command, CancellationToken cancellationToken)
    {
        var article = await _context.Articles.Include(a => a.Authors).FirstOrDefaultAsync(a => a.Id == command.ArticleId, cancellationToken);

        if(article == null)
        {
            return ResultWrap.Failure(new Error("Article.NotFound", "Failed to get article with given Id."));
        }

        var author = await _context.Authors.FindAsync(command.AuthorId);

        if (author == null)
        {
            return ResultWrap.Failure(new Error("Author.NotFound", "Failed to get author with provided Id."));
        }

        article.Authors.Remove(author);
        article.UpdateTimeMark();

        await _context.SaveChangesAsync(cancellationToken);

        return ResultWrap.Success();
    }
}
