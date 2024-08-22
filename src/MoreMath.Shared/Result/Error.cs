namespace MoreMath.Shared.Result;

public record Error(string Code, string? Message=null)
{
    public static readonly Error None = new("");

    public static Error Validation(string type, string property, string message) => new($"Validation.{type}.{property}", message);

    public static class Author
    {
        public const int Unspecified = 100;
        public const int NotFound = 101;
        public const int CreateError = 102;
    }

    public static class Article
    {
        public const int Unspecified = 200;
        public const int NotFound = 201;
        public const int CreateError = 202;
    }
}
