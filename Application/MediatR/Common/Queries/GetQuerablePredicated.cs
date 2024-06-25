using Domain.Entities.Base;
using Domain.Interfaces.Common;
using MediatR;
using System.Linq.Expressions;

namespace Application.Common.Queries;

public class GetQuerablePredicated<TEntity> : IRequest<IQueryable<TEntity>> where TEntity : EntityBase
{
    public Expression<Func<TEntity, bool>> Predicated { get; set; }
}

public class GetQuerablePredicatedHandler<TEntity> : IRequestHandler<GetQuerablePredicated<TEntity>, IQueryable<TEntity>> where TEntity : EntityBase
{
    private readonly IRepository<TEntity> _repository;

    public GetQuerablePredicatedHandler(IRepository<TEntity> repository)
    {
        _repository = repository;
    }
    public async Task<IQueryable<TEntity>> Handle(GetQuerablePredicated<TEntity> request, CancellationToken cancellationToken)
    {
        return _repository.GetAllQuerable().Where(request.Predicated);
    }
}
