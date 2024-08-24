using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Authors.Queries;

public record GetAuthorArticlesQuery(int id) : IRequest<ResultWrap<IEnumerable<ArticleDto>>>;

public class GetAuthorArticlesHandler(IUnitOfWork unitOfWork):
    AbstractHandler<GetAuthorArticlesQuery, ResultWrap<IEnumerable<ArticleDto>>>(unitOfWork)
{

    public override async Task<ResultWrap<IEnumerable<ArticleDto>>> Handle(GetAuthorArticlesQuery request, CancellationToken cancellationToken)
    {
        var author = await _unitOfWork.AuthorRepo.FindAsync(request.id);

        if(author == null)
        {
            return ResultWrap.Failure(new Error("Author.NotFound", "Failed to get author with given Id."));
        }

        var articles = await _unitOfWork.ArticleRepo.GetFilteredAsync(a => a.Authors.Contains(author));

        return ResultWrap<IEnumerable<ArticleDto>>.Success(articles.Select(a => a.ToDto()));
    }
}
