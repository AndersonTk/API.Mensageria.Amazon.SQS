using Domain.Interfaces;
using Domain.Interfaces.Base;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infra.Data.Repositories.Base;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private IDbContextTransaction _transaction;

    public UnitOfWork(ApplicationDbContext dbContext,
                      ICategoryRepository categoryRepository,
                      IProductRepository productRepository)
    {
        _dbContext = dbContext;
        Category = categoryRepository;
        Product = productRepository;
    }

    public ICategoryRepository Category { get; set; }
    public IProductRepository Product { get; set; }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public void Atach<T>(T entity) where T : class
    {
        _dbContext.Entry(entity).State = EntityState.Detached;
        _dbContext.Entry(entity).State = EntityState.Modified;
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _transaction.CommitAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _transaction.RollbackAsync();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
    }
}
