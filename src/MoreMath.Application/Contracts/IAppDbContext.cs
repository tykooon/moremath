using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MoreMath.Core.Entities;

namespace MoreMath.Application.Contracts;

public interface IAppDbContext
{
    DbSet<Author> Authors { get; }
    DbSet<Article> Articles { get; }
    DbSet<User> Users { get; }
    DbSet<Comment> Comments { get; }
    DbSet<Category> Categories { get; }
    DbSet<HebWord> HebWords { get; }
    DbSet<Tag> Tags { get; }
    DbSet<TestLessonOrder> TestLessonOrders { get; }
    DatabaseFacade DB { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task<int> SaveChangesAsync();
}
