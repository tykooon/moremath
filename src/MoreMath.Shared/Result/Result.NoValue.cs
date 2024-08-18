using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MoreMath.Shared.Result;

public class Result: Result<Result>, IResult
{
    private Result(bool isSuccessfull, Error[] errors) :
        base(isSuccessfull, errors)
    {
        _resultValue = this;
    }

    public static Result Success() => new(true, []);
    public new static Result Failure(Error[] errors) => new(false, errors);
}
