using AutoMapper;
using Common.MediatR.Commands;
using Domain.Contracts.Base;
using Domain.Entities.Base;
using MediatR;

namespace Application.MediatR.Common.Commands;

public class OrchestratorCommandHandler<TEntity, TComand>
    : IRequestHandler<TComand, TEntity>
    where TComand : ContractBase, IRequest<TEntity>
    where TEntity : EntityBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public OrchestratorCommandHandler(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<TEntity> Handle(TComand request, CancellationToken cancellationToken)
        => await _mediator.Send(new PutEntityCommand<TEntity> { Id = request.Id, Entity = _mapper.Map<TEntity>(request), UserName = request.CreateUser });
}
