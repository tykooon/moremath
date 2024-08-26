using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts.Repositories;
using MoreMath.Core.Entities;

namespace MoreMath.Infrastructure.Repositories;

public class ArticleRepository(DbContext context) : RepositoryWithDate<Article, int>(context), IArticleRepository
{
    protected override IQueryable<Article> MakeInclusions() =>
        base.MakeInclusions().Include(a => a.Authors).Include(a => a.Category).Include(a => a.Tags).AsSplitQuery();
}
