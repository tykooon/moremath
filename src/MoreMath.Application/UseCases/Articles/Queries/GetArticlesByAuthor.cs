using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Articles.Queries;

public record GetArticlesByAuthorQuery(int? Id, string? FirstName, string? LastName) : IRequest<ResultWrap<IEnumerable<ArticleDto>>>;

public class GetArticlesByAuthorHandler(IUnitOfWork unitOfWork):
    AbstractHandler<GetArticlesByAuthorQuery, ResultWrap<IEnumerable<ArticleDto>>>(unitOfWork)
{

    public override async Task<ResultWrap<IEnumerable<ArticleDto>>> Handle(GetArticlesByAuthorQuery request, CancellationToken cancellationToken)
    {

        var authors = await _unitOfWork.AuthorRepo.GetFilteredAsync(a =>
            (request.Id == null || a.Id == request.Id) &&
            (request.FirstName == null || a.FirstName == request.FirstName) &&
            (request.LastName == null || a.LastName == request.LastName));

        if (!authors.Any())
        {
            return ResultWrap.Failure(new Error("Author.NotFound", "Author with provided data was not found."));
        }

        var articles = await _unitOfWork.ArticleRepo.GetAllAsync();
            
        articles = articles.Where(a => a.Authors.Intersect(authors).Any());

        return ResultWrap<IEnumerable<ArticleDto>>.Success(articles.OrderBy(a => a.Created).Select(a => a.ToDto()));
    }
}
