using AutoMapper;
using Domain.Contracts.Base;
using MassTransit;
using MediatR;

namespace Application.Consumers.Common;

public abstract class ConsumerBase<TContract, TRequest, TResponse>
    : IConsumer<TContract>
    where TContract : ContractBase
    where TRequest : IRequest<TResponse>
    where TResponse : class
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;

    public ConsumerBase(IMediator mediator, IMapper mapper)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }
    public async Task Consume(ConsumeContext<TContract> contract)
    {
        await mediator.Send(mapper.Map<TRequest>(contract.Message));
    }
}
