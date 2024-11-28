using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;
using Microsoft.EntityFrameworkCore;
using MoreMath.Core.Entities;
using MoreMath.Dto.Responses;

namespace MoreMath.Application.UseCases.HebWords.Queries;

public record GetHebWordsQuery(
    string? Shoresh = null,
    string? NoNiqqud =null,
    string? Translation = null,
    string[]? TagList = null,
    bool HasAllTags = false) : IRequest<ResultWrap<IEnumerable<HebWordDto>>>;

public class GetHebWordsHandler(IAppDbContext context):
    AbstractHandler<GetHebWordsQuery, ResultWrap<IEnumerable<HebWordDto>>>(context)
{

    public override async Task<ResultWrap<IEnumerable<HebWordDto>>> Handle(GetHebWordsQuery query, CancellationToken cancellationToken)
    {
        int[] tags = query.TagList?.Length > 0
            ? await _context.Tags.Where(t => query.TagList.Contains(t.TagName)).Select(ta => ta.Id).ToArrayAsync(cancellationToken)
            : [];

        if (query.TagList?.Length > 0 && query.TagList.Length != tags.Length)
        {
            return ResultWrap.Failure(new Error("Tags.NotFound", "Some of provided tags do not exist."));
        }

        var baseQuery = from word in _context.HebWords.Include(t => t.Tags)
                        where (string.IsNullOrEmpty(query.Translation) ||
                            EF.Functions.Like(word.Translation, $"%{query.Translation}%"))
                        where (string.IsNullOrEmpty(query.Shoresh) ||
                            EF.Functions.Like(word.Shoresh, $"%{query.Shoresh}%"))
                        where (string.IsNullOrEmpty(query.NoNiqqud) ||
                            EF.Functions.Like(word.NoNiqqud, $"%{query.NoNiqqud}%"))
                        select word;

        IQueryable<HebWord> finalQuery = baseQuery;

        if (tags.Length > 0)
        {
            finalQuery = query.HasAllTags
                ? from word in baseQuery
                  where word.Tags.All(tag => tags.Contains(tag.Id))
                  select word
                : from word in baseQuery.Include(t => t.Tags)
                  where word.Tags.Any(tag => tags.Contains(tag.Id))
                  select word;
        }

        var result = await finalQuery
            .AsNoTracking()
            .OrderBy(w => w.Niqqud)
            .Select(h => h.ToDto())
            .ToListAsync(cancellationToken);

        return ResultWrap<IEnumerable<HebWordDto>>.Success(result);
    }
}