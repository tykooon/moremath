using MoreMath.Core.Abstracts;

namespace MoreMath.Application.Contracts;

public interface IRepository<TEntity, TKey> where TEntity : BaseEntity<TKey> where TKey : struct, IEquatable<TKey>
{
    IEnumerable<TEntity> GetAll();
    TEntity Find(TKey key);
    TKey? Add(TEntity entity);
    bool Update(TEntity entity);
    void Delete(TEntity entity);
}
