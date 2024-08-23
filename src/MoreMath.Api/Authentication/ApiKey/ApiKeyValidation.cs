using MoreMath.Api.Common;
using System.Reflection.Metadata;

namespace MoreMath.Api.Authentication.ApiKey;

public class ApiKeyValidation(IConfiguration configuration): IApiKeyValidation
{
    private readonly IConfiguration _configuration = configuration;

    public bool IsValid(string? apiKeyChallenge)
    {
        if (string.IsNullOrWhiteSpace(apiKeyChallenge))
        {
            return false;
        }

        var apiKey = _configuration[ApiConstants.APIKEY_PATH];

        return apiKey != null && apiKey.Equals(apiKeyChallenge);
    }
}
