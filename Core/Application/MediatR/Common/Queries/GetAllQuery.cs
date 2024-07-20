using Domain.Entities.Base;
using Domain.Interfaces.Common;
using MediatR;

namespace Application.Common.MediatR.Queries;

public class GetAllQuery<TEntity> : IRequest<IEnumerable<TEntity>> where TEntity : class, IEntityBase
{
}

public class GetAllQueryHandler<TEntity> : IRequestHandler<GetAllQuery<TEntity>, IEnumerable<TEntity>> where TEntity : class, IEntityBase
{
    private readonly IRepository<TEntity> _repository; // Injeção de dependência

    public GetAllQueryHandler(IRepository<TEntity> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TEntity>> Handle(GetAllQuery<TEntity> request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync();
    }
}
