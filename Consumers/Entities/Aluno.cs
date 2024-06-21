using Domain.Entities.Base;

namespace Domain.Entities;

public class Aluno : EntityBase
{
    public string Name { get; set; }
    public string CPF { get; set; }
}
