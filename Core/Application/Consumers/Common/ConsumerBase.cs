using AutoMapper;
using Domain.Contracts.Base;
using Domain.Entities.Base;
using MassTransit;
using MediatR;

namespace Application.Consumers.Common;

public abstract class ConsumerBase<TEntity, TContract, TRequest, TResponse>
    : IConsumer<TContract>
    where TEntity : EntityBase
    where TContract : ContractBase
    where TRequest : IRequest<TResponse>
    where TResponse : class
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ConsumerBase(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<TContract> contract)
        => await _mediator.Send(_mapper.Map<TRequest>(contract.Message));
}
