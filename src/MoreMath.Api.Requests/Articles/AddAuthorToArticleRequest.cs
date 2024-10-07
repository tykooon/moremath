namespace MoreMath.Api.Requests.Articles;

public record AddAuthorToArticleRequest(int? AuthorId, string? FirstName, string? LastName);