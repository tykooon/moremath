namespace MoreMath.Shared.Result;

public class ResultWrap: ResultWrap<ResultWrap>, IResultWrap
{
    private ResultWrap(bool isSuccessfull, Error[] errors) :
        base(isSuccessfull, errors)
    {
        _resultValue = this;
    }

    public static ResultWrap Success() => new(true, []);
    public new static ResultWrap Failure(params Error[] errors) => new(false, errors);
}
