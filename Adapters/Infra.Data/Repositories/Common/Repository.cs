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
        =>  await _DbSet.FindAsync(id);

    public async Task<T> GetByIdAsNoTrackingAsync(Guid id)
        => await _DbSet.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);

    public async Task<T> GetByPredicated(Expression<Func<T, bool>> predicated)
        => await _DbSet.Where(predicated).FirstOrDefaultAsync();

    public async Task<IEnumerable<T>> GetListByPredicated(Expression<Func<T, bool>> predicated)
        => await _DbSet.Where(predicated).ToListAsync();

    public async Task<IEnumerable<T>> GetAllAsync()
        => await _DbSet.ToListAsync();

    public async Task<T> AddAsync(T entity)
    {
        var save = await _DbSet.AddAsync(entity);
        return save.Entity;
    }

    public async Task<T> UpdateAsync(T entity)
        => _DbSet.Update(entity).Entity;

    public async Task DeleteAsync(T entity)
        => _DbSet.Remove(entity);

    public async Task<bool> ExistsAsync(Guid id)
        => await _DbSet.AnyAsync(e => EF.Property<Guid>(e, "Id") == id);

    public IQueryable<T> GetAllQuerable()
        => _DbSet;
}
