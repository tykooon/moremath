using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;
using Microsoft.EntityFrameworkCore;
using MoreMath.Dto.Responses;

namespace MoreMath.Application.UseCases.Articles.Queries;

public class GetArticlesPagedQuery: IRequest<ResultWrap<IEnumerable<ArticlePageItem>>>
{
    public string? CategoryName { get; set; } = null;
    public int[]? AuthorsId { get; set; } = null;
    public bool HasAllAuthors { get; set; } = false;
    public string? Title { get; set; } = null;
    public string[]? TagList { get; set; } = null;
    public bool HasAllTags { get; set; } = false;
    public string? OrderBy { get; set; } = null;
    public bool Decsending { get; set; } = false;
    public int Start { get; set; } = 0;
    public int Take { get; set; } = 5;

}

public class GetArticlesPagedHandler(IAppDbContext context):
    AbstractHandler<GetArticlesPagedQuery, ResultWrap<IEnumerable<ArticlePageItem>>>(context)
{

    public override async Task<ResultWrap<IEnumerable<ArticlePageItem>>> Handle(GetArticlesPagedQuery query, CancellationToken cancellationToken)
    {
        var whereTagClause = (query.TagList != null && query.TagList.Length > 0)
            ? $"AND (ta.TagName IN ('{string.Join("', '", query.TagList)}'))"
            : "";
        var whereAuthorsClause = (query.AuthorsId != null && query.AuthorsId.Length > 0)
            ? $"AND (au.Id IN ('{string.Join("', '", query.AuthorsId)}'))"
            : "";

        var havingTagClause = query.HasAllTags && query.TagList?.Length > 0
            ? $"COUNT(DISTINCT ta.Id) = {query.TagList.Length}"
            : "";

        var havingAuthorClause = query.HasAllAuthors && query.AuthorsId?.Length > 0
            ? $"COUNT(DISTINCT au.Id) = {query.AuthorsId.Length}"
            : "";

        var havingClause = (havingTagClause.Length > 0, havingAuthorClause.Length > 0) switch
        {
            (true, true) => $"HAVING ({havingTagClause}) AND ({havingAuthorClause})",
            (true, false) => $"HAVING ({havingTagClause})",
            (false, true) => $"HAVING ({havingAuthorClause})",
            _ => ""
        };

        var orderClause = query.OrderBy?.ToLower() switch
        {
            "created" => "ORDER BY res.Created ~, res.Id",
            "title" => "ORDER BY res.Title ~, res.Id",
            "random" => "ORDER BY RAND()",
            _ => "ORDER BY res.Id ~"
        };

        orderClause = orderClause.Replace("~", query.Decsending ? " DESC" : " ASC");

        var rawQuery =
           $"""
            SELECT res.Id, res.Title, res.CategoryId, ca2.CategoryName, res.BodyUri, res.Abstract, res.Created, res.Modified, res.ImageUri, res.Slug, GROUP_CONCAT(DISTINCT au2.Id SEPARATOR ',') AS AuthorsIdConcat, GROUP_CONCAT(DISTINCT CONCAT(au2.FirstName, ' ' ,au2.LastName) SEPARATOR ',') AS AuthorsConcat, GROUP_CONCAT(DISTINCT ta2.TagName SEPARATOR ',') AS TagsConcat, COUNT(*) OVER() AS Total
            FROM
            (
                SELECT ar.Id, ar.Title, ar.CategoryId, ar.BodyUri, ar.Abstract, ar.Created, ar.Modified, ar.ImageUri, ar.Slug 
                FROM Articles AS ar
            	LEFT JOIN ArticleTag AS arta
                    ON ar.Id = arta.ArticlesId
            	LEFT JOIN Tags AS ta
                    ON arta.TagsId = ta.Id
                LEFT JOIN ArticleAuthor AS arau
                    ON ar.Id = arau.ArticlesId
            	LEFT JOIN Authors AS au
                    ON arau.AuthorsId = au.Id
            	LEFT JOIN Categories AS ca
                    ON ca.Id = ar.CategoryId
            	WHERE (ar.Title LIKE '%{query.Title}%')
                    AND (ca.CategoryName LIKE '%{query.CategoryName}%')
                    {whereAuthorsClause}
            	  	{whereTagClause}
            	GROUP BY ar.Id
            	{havingClause}
            ) AS res
            LEFT JOIN ArticleTag AS arta2
                ON res.Id = arta2.ArticlesId
            LEFT JOIN Tags AS ta2
                ON arta2.TagsId = ta2.Id
            LEFT JOIN ArticleAuthor AS arau2
                ON res.Id = arau2.ArticlesId
            LEFT JOIN Authors AS au2
                ON arau2.AuthorsId = au2.Id
            LEFT JOIN Categories AS ca2
                ON ca2.Id = res.CategoryId
            GROUP BY res.Id
            {orderClause}
            """;

        var result = await _context.DB
            .SqlQueryRaw<ArticlePageItem>(rawQuery)
            .AsNoTracking()
            .Skip(query.Start)
            .Take(query.Take)
            .ToListAsync(cancellationToken);

        return ResultWrap<IEnumerable<ArticlePageItem>>.Success(result);
    }
}