using Common.MediatR.Commands;
using Domain.Contracts.Base;
using Domain.Entities.Base;
using MediatR;

namespace Application.MediatR.Common.Commands;

public class DeleteCommandHandler<TEntity, TComand>
    : IRequestHandler<TComand, bool>
    where TComand : ContractIdBase, IRequest<bool>
    where TEntity : EntityBase
{
    private readonly IMediator _mediator;

    public DeleteCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<bool> Handle(TComand request, CancellationToken cancellationToken)
        => await _mediator.Send(new DeleteEntityCommand<TEntity> { Id = request.Id });
}
