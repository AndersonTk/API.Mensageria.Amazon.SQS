using Domain.Entities.Base;
using Domain.Interfaces.Common;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infra.Data.Repositories.Common;

public class Repository<T> : IRepository<T> where T : class, IEntityBase
{
    private readonly ApplicationDbContext _dbContext;
    public readonly DbSet<T> _DbSet;
    public Repository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _DbSet = _dbContext.Set<T>();
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public async Task<T> GetByIdNoTrackingAsync(Guid id)
    {
        return await _dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<T> GetByPredicated(Expression<Func<T, bool>> predicated)
        => await _DbSet.Where(predicated).FirstOrDefaultAsync();

    public async Task<IEnumerable<T>> GetListByPredicated(Expression<Func<T, bool>> predicated)
        => await _DbSet.Where(predicated).ToListAsync();

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        var save = await _dbContext.Set<T>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return save.Entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        var save = _dbContext.Set<T>().Update(entity);
        await _dbContext.SaveChangesAsync();
        return save.Entity;
    }

    public async Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _dbContext.Database.RollbackTransactionAsync();
    }
    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _dbContext.Set<T>().AnyAsync(e => EF.Property<Guid>(e, "Id") == id);
    }

    public void Attach(T entity)
    {
        _dbContext.Attach(entity);
    }

    public T Modified(T entity)
    {
        var entry = _dbContext.Entry(entity);

        if (entry.State == EntityState.Detached)
        {
            _dbContext.Attach(entity);
        }

        var createDateProperty = entry.Property("CreateDate");
        var createUserProperty = entry.Property("CreateUser");

        if (createDateProperty != null && createUserProperty != null)
        {
            createDateProperty.IsModified = false;
            createUserProperty.IsModified = false;
        }

        var save = _dbContext.Set<T>().Update(entity);
        _dbContext.SaveChanges();
        return save.Entity;
    }

    public async Task<int> CountAll()
        => await _DbSet.CountAsync();

    public IQueryable<T> GetAllQuerable()
        => _DbSet;
}
