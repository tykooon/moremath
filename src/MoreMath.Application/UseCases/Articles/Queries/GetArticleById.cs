using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Articles.Queries;

public record GetArticleByIdQuery(int id) : IRequest<ResultWrap<ArticleDto?>>;

public class GetArticleByIdHandler(IUnitOfWork unitOfWork):
    AbstractHandler<GetArticleByIdQuery, ResultWrap<ArticleDto?>>(unitOfWork)
{

    public override async Task<ResultWrap<ArticleDto?>> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
    {
        var article = await _unitOfWork.ArticleRepo.FindAsync(request.id);

        return article == null
            ? ResultWrap<ArticleDto?>.Failure(new Error("Author.NotFound", "Failed to get author with given Id."))
            : ResultWrap<ArticleDto?>.Success(article.ToDto());
    }
}
