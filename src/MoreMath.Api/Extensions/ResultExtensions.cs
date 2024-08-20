using MoreMath.Shared.Result;
using HttpResult = Microsoft.AspNetCore.Http.IResult; 

namespace MoreMath.Api.Extensions;

public static class ResultExtensions
{
    public static HttpResult ToHttpResult(this ResultWrap result) =>
        result.IsSuccessfull
            ? Results.Ok()
            : Results.BadRequest(result.Errors);

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
