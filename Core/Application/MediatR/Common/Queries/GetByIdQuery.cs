using AutoMapper;
using Domain.Entities.Base;
using Domain.Interfaces.Common;
using MediatR;
using RS = Resources.Common;

namespace Application.Common.MediatR.Queries;

public class GetByIdQuery<TEntity> : IRequest<TEntity> where TEntity : class, IEntityBase
{
    public Guid Id { get; set; }
}

public class GetByIdQueryHandler<TEntity> : IRequestHandler<GetByIdQuery<TEntity>, TEntity> where TEntity : class, IEntityBase
{
    private readonly IMapper mapper;
    private readonly IRepository<TEntity> _repository;

    public GetByIdQueryHandler(IMapper mapper, IRepository<TEntity> repository)
    {
        this.mapper = mapper;
        this._repository = repository;
    }
    public async Task<TEntity> Handle(GetByIdQuery<TEntity> request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty) throw new Exception(RS.EXCEPTION_MSG_THE_PROPERTY_CANNOT_BE_NULL.Replace("{0}", nameof(request.Id)));

        var result = await _repository.GetByIdAsync(request.Id);

        if (result == null)
            throw new Exception(RS.EXCEPTION_MSG_NOT_FOUND.Replace("{0}", ""));

        return result;
    }
}
