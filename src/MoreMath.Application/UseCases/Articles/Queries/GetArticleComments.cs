using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;
using Microsoft.EntityFrameworkCore;

namespace MoreMath.Application.UseCases.Articles.Queries;

public record GetArticleCommentsQuery(int id) : IRequest<ResultWrap<IEnumerable<CommentDto>>>;

public class GetArticleCommentsHandler(IAppDbContext context):
    AbstractHandler<GetArticleCommentsQuery, ResultWrap<IEnumerable<CommentDto>>>(context)
{

    public override async Task<ResultWrap<IEnumerable<CommentDto>>> Handle(GetArticleCommentsQuery request, CancellationToken cancellationToken)
    {
        var article = await _context.Articles.FindAsync([ request.id ], cancellationToken);

        if (article == null) 
        {
            ResultWrap.Failure(new Error("Article.NotFound", "Failed to get article with given Id."));
        }

        var comments = await _context.Comments
            //.Include(c => c.User)
            .Where(c => c.ArticleId == request.id)
            .ToListAsync(cancellationToken);

        return ResultWrap<IEnumerable<CommentDto>>.Success(comments.Select(a => a.ToDto()));
    }
}
