using Microsoft.EntityFrameworkCore;
using ProfilesApi.Domain.Entities;
using ProfilesApi.Domain.Interfaces;

namespace ProfilesApi.Infrastructure;

public class GenericRepository<T>(ProfilesDbContext context) : IGenericRepository<T> where T : SoftDeletableEntity
{
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbSet.FindAsync([id], cancellationToken);

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbSet.AsNoTracking().ToListAsync(cancellationToken);

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        => await _dbSet.AddAsync(entity, cancellationToken);

    public void Update(T entity)
        => _dbSet.Update(entity);

    public void Delete(T entity)
    {
        entity.IsDeleted = true;
        entity.DeletedOnUtc = DateTime.UtcNow;
        _dbSet.Update(entity);
    }
}
