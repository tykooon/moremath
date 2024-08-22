using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;


namespace MoreMath.Application.UseCases.Articles.Queries;

public record GetArticlesQuery(string? CategoryName = null, string[]? TagList = null) : IRequest<ResultWrap<IEnumerable<ArticleDto>>>;

public class GetArticlesHandler(IUnitOfWork unitOfWork):
    AbstractHandler<GetArticlesQuery, ResultWrap<IEnumerable<ArticleDto>>>(unitOfWork)
{

    public override async Task<ResultWrap<IEnumerable<ArticleDto>>> Handle(GetArticlesQuery request, CancellationToken cancellationToken)
    {
        var articles = await _unitOfWork.ArticleRepo.GetAllAsync();

        if(request.CategoryName != null)
        {
            articles = articles.Where(a => a.Category?.CategoryName == request.CategoryName);
        }

        if(request.TagList != null && request.TagList.Length != 0)
        {
            articles = articles.Where(a => a.Tags.Select(t => t.TagName).Intersect(request.TagList).Any());
        }

        var response = articles.OrderBy(a => a.Created).Select(a => a.ToDto());
        return ResultWrap<IEnumerable<ArticleDto>>.Success(response);
    }
}
