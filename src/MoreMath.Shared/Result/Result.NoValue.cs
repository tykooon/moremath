namespace MoreMath.Shared.Result;

public class Result: Result<Result>
{
    private Result(bool isSuccessfull, params Error[] errors) :
        base(isSuccessfull, errors)
    {
        _resultValue = this;
    }

    public static Result Success() => new(true);
    public new static Result Failure(Error error) => new(false, error);

}
