using Domain.Contracts.Base;

namespace Domain.Contracts;

public class ProductContract : ContractBase
{
    public string Name { get; set; }
    public Guid CategoryId { get; set; }
}

public class ProductDeleteContract : ContractIdBase
{ }
