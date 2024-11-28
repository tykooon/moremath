using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;
using Microsoft.EntityFrameworkCore;

namespace MoreMath.Application.UseCases.Articles.Queries;

public record GetArticleBySlugQuery(string Slug) : IRequest<ResultWrap<ArticleDto?>>;

public class GetArticleBySlugHandler(IAppDbContext context):
    AbstractHandler<GetArticleBySlugQuery, ResultWrap<ArticleDto?>>(context)
{

    public override async Task<ResultWrap<ArticleDto?>> Handle(GetArticleBySlugQuery request, CancellationToken cancellationToken)
    {
        var article = await _context.Articles.AsNoTracking()
            .Include(a => a.Authors)
            .Include(a => a.Tags)
            .Include(a => a.Category)
            .FirstOrDefaultAsync(a => a.Slug == request.Slug, cancellationToken);

        return article == null
            ? ResultWrap<ArticleDto?>.Failure(new Error("Article.NotFound", "Failed to get article with given Slug."))
            : ResultWrap<ArticleDto?>.Success(article.ToDto());
    }
}
