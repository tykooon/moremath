namespace MoreMath.Dto.Dtos;

public record AuthorDto(
    int Id,
    string FirstName,
    string LastName,
    string? AvatarUrl,
    string Info,
    string ShortBio,
    string? Phone,
    string? Email,
    string? WhatsApp,
    string? Telegram,
    string? Website,
    string? Options,
    DateTime Created,
    DateTime Modified);
