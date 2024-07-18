using AutoMapper;
using Domain.Contracts.Base;
using Domain.Interfaces.Common;

namespace SqlTableDependency.ServiceBroken.Base;
public abstract class SubscribeTableBase<TContract> where TContract : ContractBase
{
    public readonly IEventBusInterface<TContract> _bus;
    public readonly IMapper _mapper;

    public SubscribeTableBase(IEventBusInterface<TContract> bus, IMapper mapper)
    {
        _bus = bus;
        _mapper = mapper;
    }
}
