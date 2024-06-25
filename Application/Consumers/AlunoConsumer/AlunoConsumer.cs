using Application.Consumers.Common;
using Application.Requests;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using MediatR;

namespace Application.Consumers;

public class AlunoConsumer : ConsumerBase<Aluno, AlunoContract, AlunoRequest, Aluno>
{
    public AlunoConsumer(IMediator mediator, IMapper mapper) : base(mediator, mapper)
    {
    }
}
