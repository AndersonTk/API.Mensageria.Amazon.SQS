using Domain.Entities;
using Domain.Interfaces;
using Infra.Data.Context;
using Infra.Data.Repositories.Common;

namespace Infra.Data.Repositories;

public class AlunoRepository : Repository<Aluno>, IAlunoRepository
{
    public AlunoRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}