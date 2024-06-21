using Domain.Interfaces.Common;
using MassTransit;

namespace Infra.Data.Repositories.Common;

public class EventBus<TContract> : IEventBusInterface<TContract> where TContract : class
{
    private readonly IBus _bus;

    public EventBus(IBus bus)
    {
        _bus = bus;
    }

    public async Task SendMessageFifo(TContract contract, string queueName)
    {
        var sendEndpoint = await _bus.GetSendEndpoint(new Uri("queue:" + queueName));

        await sendEndpoint.Send(contract, x =>
        {
            x.SetGroupId(queueName + Guid.NewGuid().ToString());
            x.TrySetDeduplicationId(Guid.NewGuid().ToString());
        });
    }
    
    public async Task SendMessage(TContract contract)
        => await _bus.Send(contract);
    
    public async Task PublishMessage(TContract contract)
        => await _bus.Publish(contract);
}
