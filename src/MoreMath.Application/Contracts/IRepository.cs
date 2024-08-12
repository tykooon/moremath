using MoreMath.Core.Abstracts;
using System.Linq.Expressions;

namespace MoreMath.Application.Contracts;

public interface IRepository<TEntity, TKey> where TEntity : BaseEntity<TKey> where TKey : struct, IEquatable<TKey>
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> FindAsync(TKey key);
    Task AddAsync(TEntity entity);
    void Delete(TEntity entity);
}
