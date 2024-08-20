namespace MoreMath.Shared.Result;

public class ResultWrap<TResult> : IResultWrap
{
    protected readonly List<Error> _errors = [];
    protected TResult? _resultValue;

    public TResult? Value => _resultValue;

    public bool IsSuccessfull { get; }
    public IEnumerable<Error> Errors => _errors;

    public Type ResultType => typeof(TResult);

    protected ResultWrap(TResult value)
    {
        IsSuccessfull = true;
        _resultValue = value;
    }

    protected ResultWrap(Error[] errors)
    {
        IsSuccessfull = false;
        _errors = [..errors];
    }

    protected ResultWrap(bool isSuccessfull, params Error[] errors)
    {
        if (isSuccessfull & errors.Length != 0)
        {
            throw new ArgumentException("Successfull result should not contain errors");
        }

        if (!isSuccessfull & errors.Length == 0)
        {
            throw new ArgumentException("Failure result should contain at least one error");
        }

        IsSuccessfull = isSuccessfull;
        _resultValue = default;
        _errors = [.. errors];
    }

    public static ResultWrap<TResult> Success(TResult value) => new (value);
    public static ResultWrap<TResult> Failure(params Error[] errors) => new(errors);

    public void AppendError(Error error)
    {
        if (IsSuccessfull)
        {
            throw new InvalidOperationException("Attempt to add error to successfull result.");
        }
        _errors.Add(error);
    }

    public static implicit operator ResultWrap<TResult>(ResultWrap result) => new (result.IsSuccessfull, result.Errors.ToArray());

}
