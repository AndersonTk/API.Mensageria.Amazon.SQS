using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Application.Configuration;
using Application.Extentions;
using Application.Requests;
using AutoMapper;
using Domain.Contracts;
using Hangfire;
using MassTransit;
using MediatR;
using Newtonsoft.Json;
using System.Transactions;

namespace Application.Consumers;

public class AlunoConsumer : IConsumer<AlunoContract>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IBus bus;

    public AlunoConsumer(IMediator mediator, IMapper mapper, IBus bus)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.bus = bus;
    }

    public async Task Consume(ConsumeContext<AlunoContract> contract)
    {
        try
        {
            var groupId = contract.Headers;
            Console.WriteLine($"Consumed message with GroupId: {groupId}");

            await mediator.Send(mapper.Map<AlunoRequest>(contract.Message));
        }
        catch (Exception ex)
        {
            //// Aqui ajustamos para anexar o sufixo -7edu_error ou -legado_error ao nome base da fila
            //var baseQueueName = QueueNames.AlunoQueue.EnviromentName(); // Ajuste conforme o contexto ou extraia de algum lugar dinamicamente
            //var errorQueueSuffix = contract.Message.SourceEnum == SourceEnum.Producer1 ? "-producer1_error" : "-producer2_error";
            //var errorQueueName = baseQueueName + errorQueueSuffix;
            //await SendToErrorQueue(errorQueueName, contract.Message, ex.Message);
        }
    }

    private async Task SendToErrorQueue(string queueName, AlunoContract message, string error)
    {
        var credentials = new BasicAWSCredentials("AKIA4USGFP4VVQTRFBLO", "wp1XYNociWFTXGc4EiAJMVh2lSnWu4UoZ17He7+E");
        var config = new AmazonSQSConfig() { RegionEndpoint = RegionEndpoint.USEast2 };  // Ajuste conforme necessário
        var client = new AmazonSQSClient(credentials, config);

        string queueUrl;

        try
        {
            var getQueueUrlResponse = await client.GetQueueUrlAsync(queueName);
            queueUrl = getQueueUrlResponse.QueueUrl;
        }
        catch (AmazonSQSException ex) when (ex.ErrorCode == "AWS.SimpleQueueService.NonExistentQueue")
        {
            // A fila não existe, vamos criá-la
            var createQueueRequest = new CreateQueueRequest
            {
                QueueName = queueName,
                Attributes = new Dictionary<string, string>
                {
                    {"VisibilityTimeout", "40" }
                }
            };
            var createQueueResponse = await client.CreateQueueAsync(createQueueRequest);
            queueUrl = createQueueResponse.QueueUrl;
        }

        await client.SendMessageAsync(new SendMessageRequest
        {
            QueueUrl = queueUrl,
            MessageBody = JsonConvert.SerializeObject(message),
            MessageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                {
                    "MessageType", new MessageAttributeValue
                    {
                        DataType = "String",
                        StringValue = message.GetType().Name  // Tipo da mensagem como uma string
                    }
                }
                ,{
                    "MT-Fault-Message", new MessageAttributeValue
                    {
                        DataType = "String",
                        StringValue = error  // Tipo da mensagem como uma string
                    }
                }
            }
            //MessageGroupId = $"{((char)message.SourceEnum)}-ErrorGroup"  // Necessário para filas FIFO
        });
    }
}

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
        //endpointConfigurator.UseMessageRetry(a => a.Incremental(3,
        //                        TimeSpan.FromSeconds(10),
        //                        TimeSpan.FromSeconds(10)));
    }
}