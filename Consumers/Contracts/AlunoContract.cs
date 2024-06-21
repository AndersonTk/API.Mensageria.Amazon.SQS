using MassTransit;

namespace Domain.Contracts;

[MessageUrn(nameof(AlunoContract))]
public class AlunoContract
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string CPF { get; set; }
    public string CreateUser { get; set; }
    public SourceEnum SourceEnum { get; set; }
}
