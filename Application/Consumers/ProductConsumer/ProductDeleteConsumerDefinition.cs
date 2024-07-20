using Application.Consumers.Configuration;
using Application.Extentions;
using MassTransit;
using System.Transactions;

namespace Application.Consumers;

public class ProductDeleteConsumerDefinition : ConsumerDefinition<ProductDeleteConsumer>
{
    public ProductDeleteConsumerDefinition()
    {
        ConcurrentMessageLimit = 2;
        Endpoint(x => x.Name = QueueNames.ProductDelete.EnviromentName());
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<ProductDeleteConsumer> consumerConfigurator)
    {
        //endpointConfigurator.UseMessageRetry(r => r.Immediate(2));
        endpointConfigurator.UseTransaction(x =>
        {
            x.Timeout = TimeSpan.FromSeconds(90);
            x.IsolationLevel = IsolationLevel.ReadCommitted;
        });
    }
}