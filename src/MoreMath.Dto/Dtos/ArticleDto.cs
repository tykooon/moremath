namespace MoreMath.Dto.Dtos;

public record ArticleDto(
    int Id,
    string Title,
    string Abstract,
    string BodyUri,
    int[] AuthorsId,
    int CategoryId,
    string? CategoryName,
    string[] Tags,
    DateTime Created,
    DateTime Modified);
