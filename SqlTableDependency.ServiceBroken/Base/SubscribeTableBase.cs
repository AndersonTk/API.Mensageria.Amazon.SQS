using AutoMapper;
using Domain.Interfaces.Common;

namespace SqlTableDependency.ServiceBroken.Base;
public abstract class SubscribeTableBase
{
    public readonly IEventBusInterface _bus;
    public readonly IMapper _mapper;

    public SubscribeTableBase(IEventBusInterface bus, IMapper mapper)
    {
        _bus = bus;
        _mapper = mapper;
    }
}
