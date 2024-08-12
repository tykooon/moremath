namespace MoreMath.Application.Dtos;

public record AuthorDto(
    int Id,
    string FirstName,
    string LastName,
    string? AvatarUrl,
    string Info,
    string ShortBio,
    DateTime Created,
    DateTime Modified);
