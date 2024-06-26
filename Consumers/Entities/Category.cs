using Domain.Entities.Base;

namespace Domain.Entities;

public class Category : EntityBase
{
    public string Name { get; set; }

    public ICollection<Product> Products { get; set; }
}
