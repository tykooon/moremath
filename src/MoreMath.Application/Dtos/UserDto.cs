namespace MoreMath.Application.Dtos;

public record UserDto(
    int Id,
    string Username,
    bool IsActive,
    int AuthorId,
    DateTime Created,
    DateTime Modified);
