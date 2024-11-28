using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;
using Microsoft.EntityFrameworkCore;

namespace MoreMath.Application.UseCases.Articles.Queries;

public record GetArticleTagsQuery(int id) : IRequest<ResultWrap<IEnumerable<TagDto>>>;

public class GetArticleTagsHandler(IAppDbContext context):
    AbstractHandler<GetArticleTagsQuery, ResultWrap<IEnumerable<TagDto>>>(context)
{

    public override async Task<ResultWrap<IEnumerable<TagDto>>> Handle(GetArticleTagsQuery request, CancellationToken cancellationToken)
    {
        var article = await _context.Articles.Include(a => a.Tags).FirstOrDefaultAsync(a => a.Id ==request.id, cancellationToken);

        return article == null
            ? ResultWrap.Failure(new Error("Article.NotFound", "Failed to get article with given Id."))
            : ResultWrap<IEnumerable<TagDto>>.Success(article.Tags.Select(t => t.ToDto()));
    }
}
