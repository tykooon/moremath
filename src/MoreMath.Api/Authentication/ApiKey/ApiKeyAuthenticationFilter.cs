using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using MoreMath.Api.Common;
using System.Net;

namespace MoreMath.Api.Authentication.ApiKey;

public class ApiKeyAuthenticationFilter(IApiKeyValidation apiKeyValidation) : IAuthorizationFilter
{
    private readonly IApiKeyValidation _apiKeyValidation = apiKeyValidation; 

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(
            ApiConstants.APIKEY_HEADERNAME,
            out var extractedApiKey))
        {
            context.Result = new ContentResult()
            {
                Content = "ApiKey was not provided",
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
            //context.Result = new BadRequestObjectResult("ApiKey was not provided");
            return;
        }

        if (!_apiKeyValidation.IsValid(extractedApiKey))
        {
            context.Result = new ContentResult()
            {
                Content = "Provided ApiKey is not valid.",
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
            return;
        }
    }
}
