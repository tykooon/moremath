using MoreMath.Application.Contracts.Repositories;

namespace MoreMath.Application.Contracts;

public interface IUnitOfWork : IDisposable
{
    IAuthorRepository AuthorRepo { get; }
    IArticleRepository ArticleRepo { get; }
    ICommentRepository CommentRepo { get; }
    IUserRepository UserRepo { get; }
    ICategoryRepository CategoryRepo { get; }
    ITagRepository TagRepo { get; }
    ITestLessonRepository TestLessonRepo { get; }

    Task CommitAsync();
}
