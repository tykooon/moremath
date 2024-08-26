using MoreMath.Shared.Result;
using System.Net;
using HttpResult = Microsoft.AspNetCore.Http.IResult; 

namespace MoreMath.Api.Extensions;

public static class ResultExtensions
{
    public static HttpResult ToHttp(this ResultWrap result, HttpStatusCode successCode, HttpStatusCode falilureCode)
    {
        if (result.IsSuccessfull)
        {
            return successCode switch
            {
                HttpStatusCode.Accepted => Results.Accepted(),
                HttpStatusCode.NoContent => Results.NoContent(),
                _ => Results.Ok()
            };
        }
        return falilureCode switch
        {
            HttpStatusCode.NotFound => Results.NotFound(result.Errors),
            HttpStatusCode.Unauthorized => Results.Unauthorized(),
            HttpStatusCode.Forbidden => Results.Forbid(),
            _ => Results.BadRequest(result.Errors)
        };
     }

    public static HttpResult ToOkOrBadRequest(this ResultWrap result) =>
        result.ToHttp(HttpStatusCode.OK, HttpStatusCode.BadRequest);

    public static HttpResult ToOkOrNotFound(this ResultWrap result) =>
        result.ToHttp(HttpStatusCode.OK, HttpStatusCode.NotFound);

    public static HttpResult ToHttpResult<T>(this ResultWrap<T> result) =>
        result.IsSuccessfull
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Errors);

    public static HttpResult ToHttpNotFound<T>(this ResultWrap<T> result) =>
    result.IsSuccessfull
        ? Results.Ok(result.Value)
        : Results.NotFound(result.Errors);

    public static HttpResult ToHttpCreated<T>(this ResultWrap<T> result, string? location) =>
    result.IsSuccessfull
        ? Results.Created(location, result.Value)
        : Results.BadRequest(result.Errors);
}
