using MediatR;
using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Tags.Queries;

public record GetArticlesByTagQuery(int ArticleId): IRequest<ResultWrap<IEnumerable<ArticleDto>>>;

public class GetArticlesByTagHandler(IAppDbContext context) :
    AbstractHandler<GetArticlesByTagQuery, ResultWrap<IEnumerable<ArticleDto>>>(context)
{
    public override async Task<ResultWrap<IEnumerable<ArticleDto>>> Handle(GetArticlesByTagQuery query, CancellationToken cancellationToken)
    {
        var tag = await _context.Tags.FindAsync(query.ArticleId);
        if(tag == null)
        {
            ResultWrap.Failure(new Error("Tag.NotFound", "Tag with provided Id was not found"));
        }

        var articles = await _context.Articles.Where(a => a.Tags.Contains(tag!)).Select(a => a.ToDto()).ToListAsync(cancellationToken);

        return ResultWrap<IEnumerable<ArticleDto>>.Success(articles);
    }
}
