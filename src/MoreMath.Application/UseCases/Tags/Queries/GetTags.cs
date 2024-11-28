using MediatR;
using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Dto.Dtos;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Tags.Queries;

public record GetTagsQuery(string? SearchString, bool? IsWordTag = null, bool? IsArticleTag = null) : IRequest<ResultWrap<IEnumerable<TagDto>>>;

public class GetTagsHandler(IAppDbContext context) :
    AbstractHandler<GetTagsQuery, ResultWrap<IEnumerable<TagDto>>>(context)
{
    public override async Task<ResultWrap<IEnumerable<TagDto>>> Handle(GetTagsQuery query, CancellationToken cancellationToken)
    {
        var IsWordTagClause = query.IsWordTag == null
            ? "TRUE"
            : $"""
                 {NotIfFalse(query.IsWordTag.Value)} EXISTS (
                 SELECT 1
                 FROM HebWordTag AS ht
                 WHERE ht.TagsId = t.Id)
             """;

        var IsArticleTagClause = query.IsArticleTag == null
            ? "TRUE"
            : $"""
                 {NotIfFalse(query.IsArticleTag.Value)} EXISTS (
                 SELECT 1
                 FROM ArticleTag AS arTag
                 WHERE arTag.TagsId = t.Id)
             """;

        var rawQuery =
            $"""
             SELECT t.Id, t.TagName
             FROM Tags AS t
             WHERE (
             {IsWordTagClause}
             ) AND (
             {IsArticleTagClause}
             ) AND (
                t.TagName LIKE '%{query.SearchString}%'
             )
            """;

        var tags = await _context.DB
            .SqlQueryRaw<TagDto>(rawQuery)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return ResultWrap<IEnumerable<TagDto>>.Success(tags);
    }

    private static string NotIfFalse(bool flag) => flag ? "" : "NOT ";
}