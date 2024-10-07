using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Articles.Queries;

public record GetArticleBySlugQuery(string slug) : IRequest<ResultWrap<ArticleDto?>>;

public class GetArticleBySlugHandler(IUnitOfWork unitOfWork):
    AbstractHandler<GetArticleBySlugQuery, ResultWrap<ArticleDto?>>(unitOfWork)
{

    public override async Task<ResultWrap<ArticleDto?>> Handle(GetArticleBySlugQuery request, CancellationToken cancellationToken)
    {
        var article = (await _unitOfWork.ArticleRepo
            .GetFilteredAsync(a => a.Slug == request.slug))
            .FirstOrDefault();

        return article == null
            ? ResultWrap<ArticleDto?>.Failure(new Error("Article.NotFound", "Failed to get article with given Slug."))
            : ResultWrap<ArticleDto?>.Success(article.ToDto());
    }
}
