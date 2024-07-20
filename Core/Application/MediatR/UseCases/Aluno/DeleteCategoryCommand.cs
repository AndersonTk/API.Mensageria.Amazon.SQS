using Application.MediatR.Common.Commands;
using Domain.Contracts.Base;
using Domain.Entities;
using MediatR;

namespace Application.MediatR.UseCases;

public class DeleteCategoryCommand : ContractIdBase, IRequest<bool>
{
}

public class CategoryDeleteCommandHandler : DeleteCommandHandler<Category, DeleteCategoryCommand>
{
    public CategoryDeleteCommandHandler(IMediator mediator) : base(mediator)
    {
    }
}