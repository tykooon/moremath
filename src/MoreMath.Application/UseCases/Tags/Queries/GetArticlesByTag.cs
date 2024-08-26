using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Shared.Result;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MoreMath.Application.UseCases.Tags.Queries;

public record GetArticlesByTagQuery(int ArticleId): IRequest<ResultWrap<IEnumerable<ArticleDto>>>;

public class GetArticlesByTagHandler(IUnitOfWork unitOfWork) :
    AbstractHandler<GetArticlesByTagQuery, ResultWrap<IEnumerable<ArticleDto>>>(unitOfWork)
{
    public override async Task<ResultWrap<IEnumerable<ArticleDto>>> Handle(GetArticlesByTagQuery query, CancellationToken cancellationToken)
    {
        var tag = await _unitOfWork.TagRepo.FindAsync(query.ArticleId);
        if(tag == null)
        {
            ResultWrap.Failure(new Error("Tag.NotFound", "Tag with provided Id was not found"));
        }

        var articles = await _unitOfWork.ArticleRepo.GetFilteredAsync(a => a.Tags.Contains(tag!));

        return ResultWrap<IEnumerable<ArticleDto>>.Success(articles.Select(a => a.ToDto()));
    }
}
