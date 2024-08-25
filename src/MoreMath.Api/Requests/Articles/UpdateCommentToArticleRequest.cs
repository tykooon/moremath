namespace MoreMath.Api.Requests.Articles;

public record UpdateCommentToArticleRequest(int? UserId, string? Text, bool? IsDeleted);