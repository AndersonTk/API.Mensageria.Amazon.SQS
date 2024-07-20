using Application.MediatR.Common.Commands;
using Domain.Contracts.Base;
using Domain.Entities;
using MediatR;

namespace Application.MediatR.UseCases;
public class DeleteProductCommand : ContractIdBase, IRequest<bool>
{ }

public class DeleteProductHandler : DeleteCommandHandler<Product, DeleteProductCommand>, IRequestHandler<DeleteProductCommand, bool>
{
    public DeleteProductHandler(IMediator mediator) : base(mediator)
    {
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        => await base.Handle(request, default);
}
