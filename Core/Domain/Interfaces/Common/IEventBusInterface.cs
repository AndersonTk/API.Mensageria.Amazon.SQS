namespace Domain.Interfaces.Common;

public interface IEventBusInterface
{
    Task SendMessageFifo<TContract>(TContract contract, string queueName) where TContract : class;
    Task SendMessage<TContract>(TContract contract) where TContract : class;
    Task PublishMessage<TContract>(TContract contract) where TContract : class;
}
