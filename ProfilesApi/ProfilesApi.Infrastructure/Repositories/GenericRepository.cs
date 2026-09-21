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

    public async Task<IReadOnlyList<T>> GetAllAsync(bool isTracked = false, CancellationToken cancellationToken = default)
        => await (isTracked ? _dbSet : _dbSet.AsNoTracking())
           .ToListAsync(cancellationToken);

    public async Task Add(T entity)
        => _dbSet.Add(entity);

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Remove(T entity)
        => _dbSet.Remove(entity);
    
}
