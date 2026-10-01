using Microsoft.EntityFrameworkCore;
using ProfilesApi.Domain.Entities;
using ProfilesApi.Domain.Interfaces;
using ProfilesApi.Infrastructure.Context;
using System.Linq.Expressions;

namespace ProfilesApi.Infrastructure.Repositories;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : SoftDeletableEntity
{
    private readonly DbSet<T> _dbSet;

    protected GenericRepository(ProfilesDbContext context)
    {
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, bool isTracked = false, CancellationToken cancellationToken = default)
        => await (isTracked ? _dbSet : _dbSet.AsNoTracking())
           .FirstOrDefaultAsync(predicate, cancellationToken);

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, bool isTracked = false, CancellationToken cancellationToken = default)
        => await (isTracked ? _dbSet : _dbSet.AsNoTracking())
           .Where(predicate)
           .ToListAsync(cancellationToken);

    public void Add(T entity)
        => _dbSet.Add(entity);

    public void AddRange(IEnumerable<T> entities)
        => _dbSet.AddRange(entities);

    public void Update(T entity)
        => _dbSet.Update(entity);
    
    public void UpdateRange(IEnumerable<T> entities)
        => _dbSet.UpdateRange(entities);

    public void Remove(T entity)
        => _dbSet.Remove(entity);

    public void RemoveRange(IEnumerable<T> entities)
        => _dbSet.RemoveRange(entities);
}
