namespace MoreMath.Application.Dtos;

public record ArticleDto(
    int Id,
    string Title,
    string Abstract,
    string BodyUri,
    int[] AuthorsId,
    DateTime Created,
    DateTime Modified);
