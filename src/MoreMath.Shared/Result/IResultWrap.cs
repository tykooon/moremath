namespace MoreMath.Shared.Result;

public interface IResultWrap
{
    bool IsSuccessfull { get; }
    IEnumerable<Error> Errors { get; }
    //Type ResultType { get; }
}
