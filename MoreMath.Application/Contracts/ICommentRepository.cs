using MoreMath.Core.Entities;

namespace MoreMath.Application.Contracts;

public interface ICommentRepository : IRepository<Comment, int>
{
    public IEnumerable<Comment> GetCommentsByArticle(Article article);
}
