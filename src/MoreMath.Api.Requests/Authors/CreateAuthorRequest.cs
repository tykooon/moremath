namespace MoreMath.Api.Requests.Authors;

public record CreateAuthorRequest(
    string FirstName,
    string LastName,
    string SlugName,
    string Info,
    string ShortBio,
    string? Phone,
    string? Email,
    string? WhatsApp,
    string? Telegram,
    string? Website,
    string? Options);