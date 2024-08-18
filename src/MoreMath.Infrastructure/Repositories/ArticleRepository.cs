using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts.Repositories;
using MoreMath.Core.Entities;

namespace MoreMath.Infrastructure.Repositories;

public class ArticleRepository(DbContext context) : RepositoryWithDate<Article, int>(context), IArticleRepository
{
}
