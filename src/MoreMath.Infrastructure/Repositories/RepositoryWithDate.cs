using Microsoft.EntityFrameworkCore;
using MoreMath.Core.Abstracts;

namespace MoreMath.Infrastructure.Repositories;

public abstract class RepositoryWithDate<TEntity, TKey>(DbContext context) : Repository<TEntity, TKey>(context)
    where TEntity : EntityWithDates<TKey>
    where TKey : struct, IEquatable<TKey>
{
    public void Update(TEntity entity)
    {
        entity.UpdateTimeMark();
        _dbSet.Update(entity);
    }
}
