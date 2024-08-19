namespace MoreMath.Shared.Result;

public class Result<TResult> : IResult
{
    protected readonly List<Error> _errors = [];
    protected TResult? _resultValue;

    public TResult? Value => _resultValue;

    public bool IsSuccessfull { get; }
    public IEnumerable<Error> Errors => _errors;

    public Type ResultType => typeof(TResult);

    protected Result(TResult value)
    {
        IsSuccessfull = true;
        _resultValue = value;
    }

    protected Result(Error[] errors)
    {
        IsSuccessfull = false;
        _errors = [..errors];
    }

    protected Result(bool isSuccessfull, Error[] errors)
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

    public static Result<TResult> Success(TResult value) => new (value);
    public static Result<TResult> Failure(Error[] errors) => new(errors);
    //public static Result<TResult> Failure(string errorCode, string message) => new (new Error(errorCode,message));

    public void AppendError(Error error)
    {
        if (IsSuccessfull)
        {
            throw new InvalidOperationException("Attempt to add error to successfull result.");
        }
        _errors.Add(error);
    }

    public static implicit operator Result<TResult>(Result result) => new (result.IsSuccessfull, result.Errors.ToArray());

}
