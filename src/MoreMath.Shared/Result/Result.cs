namespace MoreMath.Shared.Result;

public class Result<TResult>
{
    protected readonly List<Error> _errors = [];
    protected TResult? _resultValue;

    public TResult? Value => _resultValue;

    public bool IsSuccessfull { get; }
    public IEnumerable<Error> Errors => _errors;

    protected Result(bool isSuccessfull, params Error[] errors)
    {
        if (isSuccessfull && errors.Length != 0)
        {
            throw new ArgumentException("Successfull result cannot contain errors.");
        }

        if (!isSuccessfull && errors.Length == 0)
        {
            throw new ArgumentException("Failure result should contain errors info.");
        }

        IsSuccessfull = isSuccessfull;
        _errors = [.. errors];
    }

    protected Result(bool isSuccessfull, TResult value, params Error[] errors) :
        this(isSuccessfull, errors) => _resultValue = value;

    public static Result<TResult> Success(TResult value) => new (true, value);
    public static Result<TResult> Failure(Error error) => new(false, error);
    public static Result<TResult> Failure(string errorCode, string message) => new (false, new Error(errorCode,message));

    public void AppendError(Error error)
    {
        if (IsSuccessfull)
        {
            throw new InvalidOperationException("Attempt to add error to successfull result.");
        }
        _errors.Add(error);
    }

    public static implicit operator Result<TResult>(Result result) => new(result.IsSuccessfull, [.. result._errors]);

}
