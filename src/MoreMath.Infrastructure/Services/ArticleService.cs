using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts.Services;
using MoreMath.Dto.Responses;

namespace MoreMath.Infrastructure.Services;

public class ArticleService(IDbContextFactory<AppDbContext> factory) : IArticleService
{
    private readonly IDbContextFactory<AppDbContext> _factory = factory;

    public async Task<IEnumerable<ArticlePageItem>> GetArticlesPagedAsync(
        string? title,
        string? categoryName,
        string[]? tags,
        bool hasAllTags,
        int[]? authorsId,
        bool hasAllAuthors,
        string? orderBy,
        bool descending,
        int start,
        int take)
    {
        using (var context = _factory.CreateDbContext())
        {
            var whereTagClause = (tags != null && tags.Length > 0)
            ? $"AND (ta.TagName IN ('{string.Join("', '", tags)}'))"
                : "";
            var whereAuthorsClause = (authorsId != null && authorsId.Length > 0)
            ? $"AND (au.Id IN ('{string.Join("', '", authorsId)}'))"
            : "";

            var havingTagClause = hasAllTags && tags?.Length > 0
            ? $"COUNT(DISTINCT ta.Id) = {tags.Length}"
            : "";

            var havingAuthorClause = hasAllAuthors && authorsId?.Length > 0
                ? $"COUNT(DISTINCT au.Id) = {authorsId.Length}"
                : "";

            var havingClause = (havingTagClause.Length > 0, havingAuthorClause.Length > 0) switch
            {
                (true, true) => $"HAVING ({havingTagClause}) AND ({havingAuthorClause})",
                (true, false) => $"HAVING ({havingTagClause})",
                (false, true) => $"HAVING ({havingAuthorClause})",
                _ => ""
            };

            var orderClause = orderBy?.ToLower() switch
            {
                "created" => "ORDER BY res.Created ~, res.Id",
                "title" => "ORDER BY res.Title ~, res.Id",
                "random" => "ORDER BY RAND()",
                _ => "ORDER BY res.Id ~"
            };

            orderClause = orderClause.Replace("~", descending ? " DESC" : " ASC");

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
                    WHERE (ar.Title LIKE '%{title}%')
                        AND (ca.CategoryName LIKE '%{categoryName}%')
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

            var result = await context.DB
                .SqlQueryRaw<ArticlePageItem>(rawQuery)
                .AsNoTracking()
                .Skip(start)
                .Take(take)
                .ToListAsync();

            return result;
        }
    }
}
