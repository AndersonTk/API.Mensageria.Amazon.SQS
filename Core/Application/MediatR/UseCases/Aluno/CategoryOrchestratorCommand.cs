using Application.MediatR.Common.Commands;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using MediatR;

namespace Application.Requests;

public class CategoryOrchestratorCommand : CategoryContract, IRequest<Category>
{
}

public class CategoryOrchestratorCommandHandler : OrchestratorCommandHandler<Category, CategoryOrchestratorCommand>
{
    public CategoryOrchestratorCommandHandler(IMediator mediator, IMapper mapper) : base(mediator, mapper)
    {
    }
}
