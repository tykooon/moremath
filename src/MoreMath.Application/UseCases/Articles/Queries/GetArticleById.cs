using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;
using Microsoft.EntityFrameworkCore;

namespace MoreMath.Application.UseCases.Articles.Queries;

public record GetArticleByIdQuery(int id) : IRequest<ResultWrap<ArticleDto?>>;

public class GetArticleByIdHandler(IAppDbContext context):
    AbstractHandler<GetArticleByIdQuery, ResultWrap<ArticleDto?>>(context)
{

    public override async Task<ResultWrap<ArticleDto?>> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
    {
        var article = await _context.Articles.AsNoTracking()
            .Include(a => a.Authors)
            .Include(a => a.Tags)
            .Include(a => a.Category)
            .FirstOrDefaultAsync(a => a.Id == request.id, cancellationToken);

        return article == null
            ? ResultWrap<ArticleDto?>.Failure(new Error("Article.NotFound", "Failed to get article with given Id."))
            : ResultWrap<ArticleDto?>.Success(article.ToDto());
    }
}
