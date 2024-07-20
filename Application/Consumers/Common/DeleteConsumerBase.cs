using AutoMapper;
using Domain.Contracts.Base;
using Domain.Entities.Base;
using MassTransit;
using MediatR;

namespace Application.Consumers.Common;

public abstract class DeleteConsumerBase<TEntity, TContract, TRequest, TResponse>
    : IConsumer<TContract>
    where TEntity : EntityBase
    where TContract : ContractIdBase
    where TRequest : IRequest<TResponse>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public DeleteConsumerBase(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<TContract> contract)
        => await _mediator.Send(_mapper.Map<TRequest>(contract.Message));
}
