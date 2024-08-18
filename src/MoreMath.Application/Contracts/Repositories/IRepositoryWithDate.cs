using MoreMath.Core.Abstracts;

namespace MoreMath.Application.Contracts.Repositories;

public interface IRepositoryWithDate<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : EntityWithDates<TKey>
    where TKey : struct, IEquatable<TKey>
{
    void Update(TEntity entity);
}
