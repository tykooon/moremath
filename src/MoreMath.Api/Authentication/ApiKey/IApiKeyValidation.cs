namespace MoreMath.Api.Authentication.ApiKey;

public interface IApiKeyValidation
{
    bool IsValid(string? apiKeyChallenge);
}
