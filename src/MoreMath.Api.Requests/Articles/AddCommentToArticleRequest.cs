namespace MoreMath.Api.Requests.Articles;

public record AddCommentToArticleRequest(int? UserId, string Text);