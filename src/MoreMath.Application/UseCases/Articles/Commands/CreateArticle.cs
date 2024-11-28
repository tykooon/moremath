using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using MoreMath.Application.Contracts;
using MoreMath.Application.Contracts.Services;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Core.Entities;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Articles.Commands;

public record CreateArticleCommand(
    string Title,
    string Abstract,
    string BodyUri,
    string ImageUri,
    string Slug,
    int[] AuthorsId,
    int CategoryId,
    string[] Tags) : IRequest<ResultWrap<int>>;



public class CreateArticleHandler(IAppDbContext context):
    AbstractHandler<CreateArticleCommand, ResultWrap<int>>(context)
{
    public override async Task<ResultWrap<int>> Handle(CreateArticleCommand command, CancellationToken cancellationToken)
    {
        var authorList = await _context.Authors.AsNoTracking().Where(a => command.AuthorsId.Contains(a.Id)).ToListAsync(cancellationToken);
        var category = await _context.Categories.FindAsync(command.CategoryId);
        var tags = command.Tags.Length == 0
            ? []
            : await _context.Tags.AsNoTracking().Where(t => command.Tags.Contains(t.TagName)).ToListAsync(cancellationToken);

        Article article = new()
        {
            Title = command.Title,
            Abstract = command.Abstract,
            BodyUri = command.BodyUri,
            ImageUri = command.ImageUri,
            Slug = command.Slug,
            Authors = authorList,
            Category = category,
            Tags = tags, 
        };

        await _context.Articles.AddAsync(article);
        await _context.SaveChangesAsync(cancellationToken);

        return article.Id == 0
            ? ResultWrap.Failure(new Error("Article.CreateError", "Article was not created"))
            : ResultWrap<int>.Success(article.Id);
    }
}
