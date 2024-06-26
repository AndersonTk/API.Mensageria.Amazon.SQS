using Application.MediatR.Common.Commands;
using Domain.Contracts.Base;
using Domain.Entities;
using MediatR;

namespace Application.MediatR.UseCases;

public class AlunoDeleteCommand : ContractIdBase, IRequest<bool>
{
}

public class AlunoDeleteCommandHandler : DeleteCommandHandler<Category, AlunoDeleteCommand>
{
    public AlunoDeleteCommandHandler(IMediator mediator) : base(mediator)
    {
    }
}