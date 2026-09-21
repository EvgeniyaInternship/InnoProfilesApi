using Microsoft.EntityFrameworkCore;
using ProfilesApi.Domain.Entities;
using ProfilesApi.Domain.Interfaces;
using ProfilesApi.Infrastructure.Context;

namespace ProfilesApi.Infrastructure.Repositories;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : SoftDeletableEntity
{
    private readonly DbSet<T> _dbSet;

    public GenericRepository(ProfilesDbContext context)
    {
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id, bool isTracked = false, CancellationToken cancellationToken = default)
        => await (isTracked ? _dbSet : _dbSet.AsNoTracking())
           .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IEnumerable<T>> GetAllAsync(bool isTracked = false, CancellationToken cancellationToken = default)
        => await (isTracked ? _dbSet : _dbSet.AsNoTracking())
           .ToListAsync(cancellationToken);

    public void Add(T entity)
        => _dbSet.Add(entity);

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        => await _dbSet.AddRangeAsync(entities, cancellationToken);

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void UpdateRange(IEnumerable<T> entities)
        => _dbSet.UpdateRange(entities);

    public void Remove(T entity)
        => _dbSet.Remove(entity);

    public void RemoveRange(IEnumerable<T> entities)
        => _dbSet.RemoveRange(entities);
}
