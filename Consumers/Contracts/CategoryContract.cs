using Domain.Contracts.Base;
using MassTransit;

namespace Domain.Contracts;

[MessageUrn(nameof(CategoryContract))]
public class CategoryContract : ContractBase
{
    public string Name { get; set; }
}
