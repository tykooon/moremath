using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;
using Microsoft.EntityFrameworkCore;
using MoreMath.Dto.Responses;

namespace MoreMath.Application.UseCases.HebWords.Queries;

public class GetHebWordsPagedQuery : IRequest<ResultWrap<IEnumerable<HebWordPageItem>>>
{
    public string? Shoresh { get; set; } = null;
    public string? NoNiqqud { get; set; } = null;
    public string? Translation { get; set; } = null;
    public string[]? TagList { get; set; } = null; // TODO:  Check if null value is not redundant?
    public bool HasAllTags { get; set; } = false;
    public string? OrderBy { get; set; } = null;
    public bool Decsending { get; set; } = false;
    public int Start { get; set; } = 0;
    public int Take { get; set; } = 5;
}

public class GetHebWordsPagedHandler(IAppDbContext context):
    AbstractHandler<GetHebWordsPagedQuery, ResultWrap<IEnumerable<HebWordPageItem>>>(context)
{
    public override async Task<ResultWrap<IEnumerable<HebWordPageItem>>> Handle(GetHebWordsPagedQuery query, CancellationToken cancellationToken)
    {
        var whereTagClause = (query.TagList != null && query.TagList.Length > 0)
            ? $"AND (t.TagName IN ('{string.Join("', '", query.TagList)}'))"
            : "";

        var havingClause = query.HasAllTags && query.TagList?.Length >0
            ? $"HAVING COUNT(DISTINCT t.Id) = {query.TagList.Length}"
            : "";

        var orderClause = query.OrderBy?.ToLower() switch
        {
            "shoresh"     => "ORDER BY res.Shoresh, res.Id",
            "translation" => "ORDER BY res.Translation, res.Id",
            "noniqqud"    => "ORDER BY res.NoNiqqud, res.Id",
            "random"      => "ORDER BY RAND()",
            _             => "ORDER BY res.Id"
        };

        orderClause = orderClause.Replace(", ", query.Decsending ? " DESC, " : " ASC, ");

        var rawQuery = 
           $"""
               SELECT res.Id, res.Shoresh, res.Niqqud, res.NoNiqqud, res.ExtraForm, res.Spelling, res.StressLetter, res.Translation, res.Notes, res.ExtraInfo, GROUP_CONCAT(t2.TagName SEPARATOR ',') AS TagsConcat, COUNT(*) OVER() AS Total
               FROM ( 
                   SELECT h.Id, h.Shoresh, h.Niqqud, h.NoNiqqud, h.ExtraForm, h.Spelling, h.StressLetter, h.Translation, h.Notes, h.ExtraInfo
                   FROM
                       HebWords h
                   LEFT JOIN 
                       HebWordTag ht ON h.Id = ht.HebWordsId
                   LEFT JOIN 
                       Tags t ON ht.TagsId = t.Id
                   WHERE (h.NoNiqqud LIKE '%{query.NoNiqqud}%') 
                     AND (h.Shoresh LIKE '%{query.Shoresh}%')
                     AND (h.Translation LIKE '%{query.Translation}%')
                     {whereTagClause}
                   GROUP BY 
                       h.Id
                   {havingClause}
                ) AS res
                LEFT JOIN 
                    HebWordTag ht2 ON res.Id = ht2.HebWordsId
                LEFT JOIN 
                    Tags t2 ON ht2.TagsId = t2.Id            
                GROUP BY 
                    res.Id
                {orderClause}
            """;

        var result = await _context.DB
            .SqlQueryRaw<HebWordPageItem>(rawQuery)
            .AsNoTracking()
            .Skip(query.Start)
            .Take(query.Take)
            .ToListAsync(cancellationToken);

        return ResultWrap<IEnumerable<HebWordPageItem>>.Success(result);
    }
}