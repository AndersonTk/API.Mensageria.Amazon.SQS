using Application.MediatR.Common.Commands;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using MediatR;

namespace Application.Requests;

public class AlunoRequest : AlunoContract, IRequest<Aluno>
{
}

public class AlunoRequestHandler : OrchestratorCommandHandler<Aluno, AlunoRequest>
{
    public AlunoRequestHandler(IMediator mediator, IMapper mapper) : base(mediator, mapper)
    {
    }
}
