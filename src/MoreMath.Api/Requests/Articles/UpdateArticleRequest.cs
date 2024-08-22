namespace MoreMath.Api.Requests.Articles;

public record UpdateArticleRequest(
    string? Title,
    string? Abstract,
    string? BodyUri,
    int? CategoryId
    );
