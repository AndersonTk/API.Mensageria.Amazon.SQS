using Domain.Entities.Base;
using Domain.Validation;
using RS = Resources.Common;

namespace Domain.Entities;

public class Category : EntityBase
{
    public string Name { get; set; }

    public ICollection<Product> Products { get; set; }

    public Category() { }

    public Category(string name) : base()
    {
        Name = name;
    }

    public void Validate()
    {
        DomainExceptionValidation.When(Id == Guid.Empty, RS.DATA_ANOTATION_REQUIRED.Replace("{0}", RS.ENTITY_LBL_IDENTIFIER));
        DomainExceptionValidation.When(string.IsNullOrEmpty(Name), RS.DATA_ANOTATION_REQUIRED.Replace("{0}", RS.CATEGORY_LBL_NAME));
    }
}
