using MoreMath.Application.Contracts;
using MoreMath.Infrastructure.Repositories;

namespace MoreMath.Infrastructure;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private bool _isDisposed;
    private readonly AppDbContext _context = context;

    private AuthorRepository? _authorRepo;
    private ArticleRepository? _articleRepo;
    private CommentRepository? _commentRepo;
    private UserRepository? _userRepo;
    private CategoryRepository? _categoryRepo;
    private TagRepository? _tagRepo;

    public AppDbContext Context => _context ?? new AppDbContext();

    public IAuthorRepository AuthorRepo => _authorRepo ??= new AuthorRepository(Context);
    public IArticleRepository ArticleRepo => _articleRepo ??= new ArticleRepository(Context);
    public ICommentRepository CommentRepo => _commentRepo ??= new CommentRepository(Context);
    public IUserRepository UserRepo => _userRepo ??= new UserRepository(Context);
    public ICategoryRepository CategoryRepo => _categoryRepo ??= new CategoryRepository(Context);
    public ITagRepository TagRepo => _tagRepo ??= new TagRepository(Context);

    public async Task CommitAsync()
    {
        ObjectDisposedException.ThrowIf(_isDisposed, typeof(UnitOfWork));

        if (_context is null)
        {
            return;
        }

        await Context.SaveChangesAsync();
    }


    public void Dispose()
    {
        if (_context is null || _isDisposed)
        {
            return;
        }

        _context.Dispose();
        _isDisposed = true;
    }
}
