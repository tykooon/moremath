namespace MoreMath.Shared.Result;

public record Error(string Code, string? Message=null)
{
    public static readonly Error None = new("");
}
