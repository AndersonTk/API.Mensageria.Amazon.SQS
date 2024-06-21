using Domain.Entities.Base;
using System.Linq.Expressions;

namespace Domain.Interfaces.Common;

public interface IRepository<T> where T : class, IEntityBase
{
    Task<T> GetByIdAsync(Guid id);
    Task<T> GetByIdNoTrackingAsync(Guid id);
    Task<T> GetByPredicated(Expression<Func<T, bool>> predicated);
    Task<IEnumerable<T>> GetListByPredicated(Expression<Func<T, bool>> predicated);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    IQueryable<T> GetAllQuerable();
    Task<bool> ExistsAsync(Guid id);
    void Attach(T entity);
    T Modified(T entity);
    Task<int> CountAll();
}
