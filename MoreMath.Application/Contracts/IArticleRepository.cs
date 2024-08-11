using MoreMath.Core.Entities;

namespace MoreMath.Application.Contracts;

public interface IArticleRepository : IRepository<Article, int>
{
    public IEnumerable<Article> GetArticlesByAuthor(Author author);
}
