using Application.Consumers.Common;
using Application.MediatR.UseCases;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using MediatR;

namespace Application.Consumers;

public class ProductConsumer : ConsumerBase<Product, ProductContract, ProductOrchestratorCommand, Product>
{
    public ProductConsumer(IMediator mediator, IMapper mapper) : base(mediator, mapper)
    {
    }
}
