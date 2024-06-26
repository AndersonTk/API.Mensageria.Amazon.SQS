namespace Domain.Interfaces.Base;

public interface IUnitOfWork : IDisposable
{
    public IICategoryRepository Alunos{ get; set; }

    Task SaveChangesAsync();
    void Atach<T>(T entity) where T : class;
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
