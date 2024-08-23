using Microsoft.AspNetCore.Mvc;

namespace MoreMath.Api.Authentication.ApiKey;

public class ApiKeyAttribute : ServiceFilterAttribute
{
    public ApiKeyAttribute() : base(typeof(ApiKeyAuthenticationFilter))
    {
    }
}
