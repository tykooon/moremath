using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;
using Microsoft.EntityFrameworkCore;



namespace MoreMath.Application.UseCases.Articles.Queries;

public record GetArticlesQuery(
    string? CategoryName = null,
    int? AuthorId = null,
    string? FirstName =null,
    string? LastName = null,
    string[]? TagList = null) : IRequest<ResultWrap<IEnumerable<ArticleDto>>>;

public class GetArticlesHandler(IAppDbContext context):
    AbstractHandler<GetArticlesQuery, ResultWrap<IEnumerable<ArticleDto>>>(context)
{

    public override async Task<ResultWrap<IEnumerable<ArticleDto>>> Handle(GetArticlesQuery request, CancellationToken cancellationToken)
    {
        int? categoryId = null;
        if (request.CategoryName != null) {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryName == request.CategoryName, cancellationToken);
            if (category == null)
            {
                return ResultWrap.Failure(new Error("Category.NotFound", "Category with provided id was not found."));
            }
            categoryId = category.Id;
        }

        int[]? tagsId = null;
        if (request.TagList != null && request.TagList.Length != 0)
        {
            tagsId = await _context.Tags
                .Where(t => request.TagList.Contains(t.TagName))
                .Select(t => t.Id)
                .ToArrayAsync(cancellationToken);
        }

        int[]? authorsId = null;
        if(request.AuthorId != null || request.FirstName != null ||  request.LastName != null)
        {
            var authorsQuery = _context.Authors
                .Where(a =>
                    (request.AuthorId == null || a.Id == request.AuthorId) &&
                    (request.FirstName == null || a.FirstName == request.FirstName) &&
                    (request.LastName == null || a.LastName == request.LastName));

            authorsId = await authorsQuery.Select(a => a.Id).ToArrayAsync(cancellationToken);

            if (authorsId.Length == 0)
            {
                return ResultWrap.Failure(new Error("Author.NotFound", "Author with provided data was not found."));
            }
        }

        var articles = from art in _context.Articles.Include(a => a.Tags).Include(a => a.Authors).Include(a => a.Category)
                       where (categoryId == null || art.CategoryId == categoryId)
                       where (tagsId == null || art.Tags.Select(t => t.Id).All(tId => tagsId.Contains(tId)))
                       where (authorsId == null || art.Authors.Select(a => a.Id).All(aId => authorsId.Contains(aId)))
                       orderby art.Created
                       select art.ToDto();

        var response = await articles.ToListAsync(cancellationToken);

        return ResultWrap<IEnumerable<ArticleDto>>.Success(response);
    }
}
