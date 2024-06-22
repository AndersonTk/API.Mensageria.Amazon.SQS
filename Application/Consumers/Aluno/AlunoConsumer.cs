using Application.Consumers.Common;
using Application.Requests;
using AutoMapper;
using Domain.Contracts;
using MediatR;

namespace Application.Consumers.Aluno;

public class AlunoConsumer : ConsumerBase<AlunoContract, AlunoRequest, AlunoContract>
{
    public AlunoConsumer(IMediator mediator, IMapper mapper) : base(mediator, mapper)
    {
    }
}
