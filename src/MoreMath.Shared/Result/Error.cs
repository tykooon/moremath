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
    
    public static class Comment
    {
        public const int Unspecified = 300;
        public const int NotFound = 301;
        public const int CreateError = 302;
    }

    public static class User
    {
        public const int Unspecified = 400;
        public const int NotFound = 401;
        public const int CreateError = 402;
    }

    public static class Category
    {
        public const int Unspecified = 500;
        public const int NotFound = 501;
        public const int CreateError = 502;
    }

    public static class Tag
    {
        public const int Unspecified = 600;
        public const int NotFound = 601;
        public const int CreateError = 602;
    }

    public static class ValidationError
    {
        public const int Unspecified = 700;
        public const int Author = 701;
        public const int Article = 702;
        public const int Comment = 703;
        public const int User = 704;
        public const int Category = 705;
        public const int Tag = 706;
    }

    public static class Authorization
    {
        public const int Unspecified = 800;
        public const int NotAuthorized = 801;
        public const int NotAuthenticated = 802;
    }
}
