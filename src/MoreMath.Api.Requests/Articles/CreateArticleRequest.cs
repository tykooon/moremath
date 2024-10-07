namespace MoreMath.Api.Requests.Articles;

public record CreateArticleRequest(
    string Title,
    string Abstract,
    string BodyUri,
    string ImageUri,
    string Slug,
    int[] AuthorsId,
    int CategoryId,
    string[] Tags
    );
