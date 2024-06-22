using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Interfaces;
using Hangfire;
using MediatR;

namespace Application.Requests;

public class AlunoRequest : AlunoContract, IRequest<AlunoContract>
{
}

public class AlunoRequestHandler : IRequestHandler<AlunoRequest, AlunoContract>
{
    private readonly IMapper mapper;
    private readonly IAlunoRepository repository;

    public AlunoRequestHandler(IMapper mapper, IAlunoRepository repository)
    {
        this.mapper = mapper;
        this.repository = repository;
    }

    [Queue("consumers")]
    public async Task<AlunoContract> Handle(AlunoRequest request, CancellationToken cancellationToken)
    {
        Aluno aluno = mapper.Map<Aluno>(request);

        if (await repository.ExistsAsync(request.Id))
        {
            await repository.UpdateAsync(aluno);
            return mapper.Map<AlunoContract>(aluno);

        }
        else
        {
            await repository.AddAsync(aluno);
        }

        return mapper.Map<AlunoContract>(aluno);
    }
}
