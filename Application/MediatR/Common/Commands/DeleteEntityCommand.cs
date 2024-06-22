using Domain.Entities.Base;
using Domain.Interfaces.Base;
using Domain.Interfaces.Common;
using MediatR;

namespace Common.MediatR.Commands;

public class DeleteEntityCommand<TEntity> : IRequest<bool> where TEntity : EntityBase, IEntityBase
{
    public Guid Id { get; set; }
}

public class DeleteEntityCommandHandler<TEntity> : IRequestHandler<DeleteEntityCommand<TEntity>, bool> where TEntity : EntityBase, IEntityBase
{
    private readonly IRepository<TEntity> _write;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEntityCommandHandler(IRepository<TEntity> write, IUnitOfWork unitOfWork)
    {
        _write = write;
        _unitOfWork = unitOfWork;
    }
    public async Task<bool> Handle(DeleteEntityCommand<TEntity> request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
            throw new Exception($"{nameof(request.Id)} não encontrado");

        var entity = await _write.GetByIdAsync(request.Id);
        if (entity == null)
            throw new Exception($"{nameof(entity)} não encontrada");

        await _write.DeleteAsync(entity);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
