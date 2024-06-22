using Domain.Contracts.Base;
using MassTransit;

namespace Domain.Contracts;

[MessageUrn(nameof(AlunoContract))]
public class AlunoContract : ContractBase
{
    public string Name { get; set; }
    public string CPF { get; set; }
    public SourceEnum SourceEnum { get; set; }
}
