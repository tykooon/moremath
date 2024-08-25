namespace MoreMath.Api.Requests.Authors;

public record UpdateAuthorRequest(
    string? FirstName,
    string? LastName,
    string? AvatarUri,
    string? Info,
    string? ShortBio,
    string? Phone,
    string? Email,
    string? WhatsApp,
    string? Telegram,
    string? Website,
    string? Options);