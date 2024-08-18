namespace MoreMath.Api.Requests.Authors;

public record UpdateAuthorRequest(
    int Id,
    string? FirstName,
    string? LastName,
    string? AvatarUri,
    string? Info,
    string? ShortBio);