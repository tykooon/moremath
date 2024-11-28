namespace MoreMath.Dto.Responses;

public record ArticlePageItem(
    int Id,
    string Title,
    string Abstract,
    string BodyUri,
    string ImageUri,
    string Slug,
    string? AuthorsIdConcat,
    string? AuthorsConcat,
    int CategoryId,
    string? CategoryName,
    string? TagsConcat,
    DateTime Created,
    DateTime Modified,
    int Total);
