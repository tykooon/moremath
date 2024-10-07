namespace MoreMath.Api.Requests.Articles;

public record UpdateArticleRequest(
    string? Title,
    string? Abstract,
    string? BodyUri,
    string? ImageUri,
    string? Slug,
    int? CategoryId
    );
