using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;
using Microsoft.EntityFrameworkCore;

namespace MoreMath.Application.UseCases.Authors.Queries;

public record GetAuthorArticlesQuery(int id) : IRequest<ResultWrap<IEnumerable<ArticleDto>>>;

public class GetAuthorArticlesHandler(IAppDbContext context):
    AbstractHandler<GetAuthorArticlesQuery, ResultWrap<IEnumerable<ArticleDto>>>(context)
{

    public override async Task<ResultWrap<IEnumerable<ArticleDto>>> Handle(GetAuthorArticlesQuery request, CancellationToken cancellationToken)
    {
        var author = await _context.Authors.FindAsync(request.id);

        if(author == null)
        {
            return ResultWrap.Failure(new Error("Author.NotFound", "Failed to get author with given Id."));
        }

        var articles = await _context.Articles.Where(a => a.Authors.Contains(author)).ToListAsync(cancellationToken);

        return ResultWrap<IEnumerable<ArticleDto>>.Success(articles.Select(a => a.ToDto()));
    }
}
