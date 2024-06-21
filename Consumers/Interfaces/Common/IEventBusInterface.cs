namespace Domain.Interfaces.Common;

public interface IEventBusInterface<TContract> where TContract : class
{
    Task SendMessageFifo(TContract contract, string queueName);
    Task SendMessage(TContract contract);
    Task PublishMessage(TContract contract);
}
