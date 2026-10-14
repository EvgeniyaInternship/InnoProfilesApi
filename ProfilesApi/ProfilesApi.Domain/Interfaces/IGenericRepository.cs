using ProfilesApi.Domain.Entities;
using System.Linq.Expressions;

namespace ProfilesApi.Domain.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate, bool isTracked = false, CancellationToken cancellationToken = default);
    Task<List<T>> GetAllAsync(
        Expression<Func<T, bool>> predicate,
        int pageNumber = 0,
        int pageSize = 0,
        bool isTracked = false,
        CancellationToken cancellationToken = default);
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}