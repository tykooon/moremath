namespace MoreMath.Api.Requests.Authors;

public record CreateAuthorRequest(
    string FirstName,
    string LastName,
    string AvatarUri,
    string Info,
    string ShortBio);