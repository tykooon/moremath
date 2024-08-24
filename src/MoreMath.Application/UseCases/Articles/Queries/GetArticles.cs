using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;
using MoreMath.Core.Entities;


namespace MoreMath.Application.UseCases.Articles.Queries;

public record GetArticlesQuery(
    string? CategoryName = null,
    int? AuthorId = null,
    string? FirstName =null,
    string? LastName = null,
    string[]? TagList = null) : IRequest<ResultWrap<IEnumerable<ArticleDto>>>;

public class GetArticlesHandler(IUnitOfWork unitOfWork):
    AbstractHandler<GetArticlesQuery, ResultWrap<IEnumerable<ArticleDto>>>(unitOfWork)
{

    public override async Task<ResultWrap<IEnumerable<ArticleDto>>> Handle(GetArticlesQuery request, CancellationToken cancellationToken)
    {
        // TODO: Try to optimize this handler, (e.g. using authorService)

        IEnumerable<Author> authors = [];

        if(request.AuthorId != null || request.FirstName != null ||  request.LastName != null)
        {
            authors = await _unitOfWork.AuthorRepo.GetFilteredAsync(a =>
                (request.AuthorId == null || a.Id == request.AuthorId) &&
                (request.FirstName == null || a.FirstName == request.FirstName) &&
                (request.LastName == null || a.LastName == request.LastName));

            if (!authors.Any())
            {
                return ResultWrap.Failure(new Error("Author.NotFound", "Author with provided data was not found."));
            }
        }

        var articles = await _unitOfWork.ArticleRepo.GetAllAsync();

        if (authors.Any())
        {
            articles = articles.Where(a => a.Authors.Intersect(authors).Any());
        }

        if (request.CategoryName != null)
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
