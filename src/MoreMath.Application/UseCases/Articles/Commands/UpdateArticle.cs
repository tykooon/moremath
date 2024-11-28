using MediatR;
using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Articles.Commands;

public record UpdateArticleCommand(
    int Id,
    string? Title,
    string? Abstract,
    string? BodyUri,
    string? ImageUri,
    string? Slug,
    int? CategoryId) : IRequest<ResultWrap>;



public class  UpdateArticleHandler(IAppDbContext context):
    AbstractHandler<UpdateArticleCommand, ResultWrap>(context)
{
    public override async Task<ResultWrap> Handle(UpdateArticleCommand command, CancellationToken cancellationToken)
    {
        var article = await _context.Articles.FindAsync(command.Id);

        if (article == null)
        {
            return ResultWrap.Failure(new Error("Article.NotFound", "Failed to update article with given Id. Article wasn't found."));
        }

        article.Title = command.Title ?? article.Title;
        article.Abstract = command.Abstract ?? article.Abstract;
        article.BodyUri = command.BodyUri ?? article.BodyUri;
        article.ImageUri = command.ImageUri ?? article.ImageUri;
        article.Slug = command.Slug ?? article.Slug;
        if (command.CategoryId != null)
        {
            var category = await _context.Categories.FindAsync([command.CategoryId.Value], cancellationToken);
            if (category == null)
            {
                return ResultWrap.Failure(new Error("Category.NotFound", "Failed to update article. Provided CategoryId was not found. Article wasn't found."));
            }
            article.Category = category;
        }

        _context.Articles.Update(article);

        await _context.SaveChangesAsync(cancellationToken);
        return ResultWrap.Success();
    }
}
