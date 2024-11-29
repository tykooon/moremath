using MoreMath.Dto.Responses;

namespace MoreMath.Application.Contracts.Services;

public interface IArticleService
{
    Task<IEnumerable<ArticlePageItem>> GetArticlesPagedAsync(
        string? title,
        string? categoryName,
        string[]? tags,
        bool hasAllTags,
        int[]? authorsId,
        bool hasAllAuthors,
        string? orderBy,
        bool descending,
        int start,
        int take);
}
