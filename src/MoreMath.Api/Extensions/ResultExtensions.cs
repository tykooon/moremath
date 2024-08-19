using MoreMath.Shared.Result;
using ApiResult =  MoreMath.Shared.Result.Result;
using HttpResult = Microsoft.AspNetCore.Http.IResult; 

namespace MoreMath.Api.Extensions;

public static class ResultExtensions
{
    public static HttpResult ToHttpResult(this ApiResult result) => 
        result.IsSuccessfull
            ? Results.Ok()
            : Results.BadRequest(result.Errors);

    public static HttpResult ToHttpResult<T>(this Result<T> result) =>
        result.IsSuccessfull
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Errors);
}
