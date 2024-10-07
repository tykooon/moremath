namespace MoreMath.Api.Requests.Articles;

public record AddTagsToArticleRequest(int[]? TagsId, string[]? TagNames);