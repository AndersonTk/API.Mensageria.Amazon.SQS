using Domain.Interfaces.Common;
using MassTransit;

namespace Infra.Data.Repositories.Common;

public class EventBus : IEventBusInterface
{
    private readonly IBus _bus;

    public EventBus(IBus bus)
    {
        _bus = bus;
    }

    public async Task SendMessageFifo<TContract>(TContract contract, string queueName) where TContract : class
    {
        var sendEndpoint = await _bus.GetSendEndpoint(new Uri("queue:" + queueName));

        await sendEndpoint.Send(contract, x =>
        {
            x.SetGroupId(queueName + Guid.NewGuid().ToString());
            x.TrySetDeduplicationId(Guid.NewGuid().ToString());
        });
    }
    
    public async Task SendMessage<TContract>(TContract contract) where TContract : class
        => await _bus.Send(contract);
    
    public async Task PublishMessage<TContract>(TContract contract) where TContract: class
        => await _bus.Publish(contract);
}
