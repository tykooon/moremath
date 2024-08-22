namespace MoreMath.Api.Requests.Articles;

public record CreateArticleRequest(
    string Title,
    string Abstract,
    string BodyUri,
    int[] AuthorsId,
    int CategoryId,
    string[] Tags
    );
