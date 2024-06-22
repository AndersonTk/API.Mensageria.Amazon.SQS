using Domain.Entities.Base;
using Domain.Interfaces.Base;
using Domain.Interfaces.Common;
using MediatR;

namespace Common.MediatR.Commands;

public class PutEntityCommand<TEntity> : IRequest<TEntity> where TEntity : class, IEntityBase
{
    public Guid Id { get; set; }
    public TEntity Entity { get; set; }
    public string UserName { get; set; }
}

public class PutEntityCommandHandler<TEntity> : IRequestHandler<PutEntityCommand<TEntity>, TEntity> where TEntity : class, IEntityBase
{
    private readonly IRepository<TEntity> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public PutEntityCommandHandler(IRepository<TEntity> repository,
                                   IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task<TEntity> Handle(PutEntityCommand<TEntity> request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsNoTrackingAsync(request.Id);

        if (entity == null)
        {
            request.Entity.CreateUser = request.UserName;
            var result = await _repository.AddAsync(request.Entity);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }
        else
        {
            request.Entity.CreateUser = entity.CreateUser;
            request.Entity.UpdateUser = request.UserName;

            var result = await _repository.UpdateAsync(request.Entity);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }
    }
}
