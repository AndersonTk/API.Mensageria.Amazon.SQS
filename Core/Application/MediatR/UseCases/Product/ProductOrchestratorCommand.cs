using Application.MediatR.Common.Commands;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using MediatR;

namespace Application.MediatR.UseCases;

public class ProductOrchestratorCommand : ProductContract, IRequest<Product>
{
    public ProductOrchestratorCommand()
    {
        new ProductValidation().Validate(this);
    }
}

public class ProductOrchestratorCommandHandler : OrchestratorCommandHandler<Product, ProductOrchestratorCommand>
{
    public ProductOrchestratorCommandHandler(IMediator mediator, IMapper mapper) : base(mediator, mapper)
    {
    }
}
