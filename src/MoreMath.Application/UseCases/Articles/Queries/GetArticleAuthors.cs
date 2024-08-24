using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Articles.Queries;

public record GetArticleAuthorsQuery(int id) : IRequest<ResultWrap<IEnumerable<AuthorDto>>>;

public class GetArticleAuthorsHandler(IUnitOfWork unitOfWork):
    AbstractHandler<GetArticleAuthorsQuery, ResultWrap<IEnumerable<AuthorDto>>>(unitOfWork)
{

    public override async Task<ResultWrap<IEnumerable<AuthorDto>>> Handle(GetArticleAuthorsQuery request, CancellationToken cancellationToken)
    {
        var article = await _unitOfWork.ArticleRepo.FindAsync(request.id);

        return article == null
            ? ResultWrap.Failure(new Error("Article.NotFound", "Failed to get article with given Id."))
            : ResultWrap<IEnumerable<AuthorDto>>.Success(article.Authors.Select(a => a.ToDto()));
    }
}
