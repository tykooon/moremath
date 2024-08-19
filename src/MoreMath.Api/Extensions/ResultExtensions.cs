using MoreMath.Shared.Result;
using HttpResult = Microsoft.AspNetCore.Http.IResult; 

namespace MoreMath.Api.Extensions;

public static class ResultExtensions
{
    public static HttpResult ToHttpResult(this Result result) =>
        result.IsSuccessfull
            ? Results.Ok()
            : Results.BadRequest(result.Errors);

    public static HttpResult ToHttpResult<T>(this Result<T> result) =>
        result.IsSuccessfull
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Errors);

    public static HttpResult ToHttpCreated<T>(this Result<T> result, string? location) =>
    result.IsSuccessfull
        ? Results.Created(location, result.Value)
        : Results.BadRequest(result.Errors);
}
