using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Articles.Queries;

public record GetArticleTagsQuery(int id) : IRequest<ResultWrap<IEnumerable<TagDto>>>;

public class GetArticleTagsHandler(IUnitOfWork unitOfWork):
    AbstractHandler<GetArticleTagsQuery, ResultWrap<IEnumerable<TagDto>>>(unitOfWork)
{

    public override async Task<ResultWrap<IEnumerable<TagDto>>> Handle(GetArticleTagsQuery request, CancellationToken cancellationToken)
    {
        var article = await _unitOfWork.ArticleRepo.FindAsync(request.id);

        return article == null
            ? ResultWrap.Failure(new Error("Article.NotFound", "Failed to get article with given Id."))
            : ResultWrap<IEnumerable<TagDto>>.Success(article.Tags.Select(a => a.ToDto()));
    }
}
