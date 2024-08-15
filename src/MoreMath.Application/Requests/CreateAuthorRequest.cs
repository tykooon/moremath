namespace MoreMath.Application.Requests;

public record CreateAuthorRequest(
    string FirstName,
    string LastName,
    string AvatarUri,
    string Info,
    string ShortBio);