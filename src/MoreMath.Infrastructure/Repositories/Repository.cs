using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts;
using MoreMath.Core.Abstracts;
using System.Linq.Expressions;

namespace MoreMath.Infrastructure.Repositories;

public abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : BaseEntity<TKey>
    where TKey : struct, IEquatable<TKey>
{
    protected DbSet<TEntity> _dbSet;

    public Repository(DbContext context)
    {
        _dbSet = context.Set<TEntity>();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task<TEntity?> FindAsync(TKey key)
    {
        return await _dbSet.FindAsync(key);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await MakeInclusions().ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> predicate)
    {
        predicate ??= (x => false);
        return await MakeInclusions().Where(predicate).ToListAsync();
    }

    protected virtual IQueryable<TEntity> MakeInclusions() => _dbSet.AsQueryable();
}
