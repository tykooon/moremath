namespace MoreMath.Dto.Dtos;

public record CommentDto(
    int Id,
    int UserId,
    int ArticleId,
    string Text,
    bool IsDeleted,
    DateTime Created,
    DateTime Modified);
