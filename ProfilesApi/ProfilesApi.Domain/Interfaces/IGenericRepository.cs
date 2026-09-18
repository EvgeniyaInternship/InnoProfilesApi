using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Domain.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id, bool isTracked = false, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> GetAllAsync(bool isTracked = false, CancellationToken cancellationToken = default);
    Task Add(T entity);
    void Update(T entity);
    void Remove(T entity);
}