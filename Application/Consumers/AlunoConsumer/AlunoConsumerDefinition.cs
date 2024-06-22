using Application.Consumers.Configuration;
using Application.Extentions;
using MassTransit;
using System.Transactions;

namespace Application.Consumers.AlunoConsumer;

public class AlunoConsumerDefinition : ConsumerDefinition<AlunoConsumer>
{
    public AlunoConsumerDefinition()
    {
        ConcurrentMessageLimit = 2;
        Endpoint(x => x.Name = QueueNames.AlunoQueue.EnviromentName());
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<AlunoConsumer> consumerConfigurator)
    {
        //endpointConfigurator.UseMessageRetry(r => r.Immediate(2));
        endpointConfigurator.UseTransaction(x =>
        {
            x.Timeout = TimeSpan.FromSeconds(90);
            x.IsolationLevel = IsolationLevel.ReadCommitted;
        });
    }
}