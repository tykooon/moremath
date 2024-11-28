using MediatR;
using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Articles.Commands;

public record AddAuthorToArticleCommand(
    int ArticleId,
    int? AuthorId,
    string? FirstName,
    string? LastName): IRequest<ResultWrap>;

public class AddAuthorToArticleHandler(IAppDbContext context):
    AbstractHandler<AddAuthorToArticleCommand, ResultWrap>(context)
{
    public override async Task<ResultWrap> Handle(AddAuthorToArticleCommand command, CancellationToken cancellationToken)
    {
        var article = await _context.Articles
            .Include(a=> a.Authors)
            .FirstOrDefaultAsync(a => a.Id == command.ArticleId, cancellationToken);

        if(article == null)
        {
            return ResultWrap.Failure(new Error("Article.NotFound", "Failed to get article with given Id."));
        }

        var authors = await _context.Authors.Where(a =>
            (command.AuthorId == null || a.Id == command.AuthorId) &&
            (command.FirstName == null || a.FirstName == command.FirstName) &&
            (command.LastName == null || a.LastName == command.LastName)).AsNoTracking().ToListAsync(cancellationToken);

        if (authors.Count == 0)
        {
            return ResultWrap.Failure(new Error("Author.NotFound", "Failed to get author with provided data."));
        }

        if (authors.Count > 1)
        {
            return ResultWrap.Failure(new Error("Author.NotSpecified", "Sorry, several authors match provided data. Try to specift it."));
        }

        var author = authors.First();

        if (!article.Authors.Contains(author))
        {
            article.Authors.Add(author);
            article.UpdateTimeMark();
            await _context.SaveChangesAsync(cancellationToken);
        }

        return ResultWrap.Success();
    }
}
