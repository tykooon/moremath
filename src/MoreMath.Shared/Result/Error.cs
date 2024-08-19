namespace MoreMath.Shared.Result;

public record Error(string Code, string? Message=null)
{
    public static readonly Error None = new("");

    public static Error Validation(string type, string property, string message) => new($"Validation.{type}.{property}", message);
}
