namespace MoreMath.Application.Dtos;

public record Comment(
    int Id,
    int UserId,
    int ArticleId,
    string Text,
    bool IsDeleted,
    DateTime Created,
    DateTime Modified);
