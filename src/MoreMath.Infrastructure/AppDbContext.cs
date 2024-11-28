using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MoreMath.Application.Contracts;
using MoreMath.Core.Entities;
using System.Reflection;

namespace MoreMath.Infrastructure;

public class AppDbContext: DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    public AppDbContext() : base()
    { }

    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<HebWord> HebWords => Set<HebWord>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<TestLessonOrder> TestLessonOrders => Set<TestLessonOrder>();

    public DatabaseFacade DB => base.Database;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public Task<int> SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }
}
