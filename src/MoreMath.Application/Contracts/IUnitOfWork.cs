namespace MoreMath.Application.Contracts;

public interface IUnitOfWork : IDisposable
{
    IAuthorRepository AuthorRepo { get; }
    IArticleRepository ArticleRepo { get; }
    ICommentRepository CommentRepo { get; }
    IUserRepository UserRepo { get; }
    ICategoryRepository CategoryRepo { get; }
    ITagRepository TagRepo { get; }

    Task CommitAsync();
}
