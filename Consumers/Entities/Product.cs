using Domain.Entities.Base;
using Domain.Validation;
using RS = Resources.Common;

namespace Domain.Entities;

public class Product : EntityBase
{
    public string Name { get; set; }

    public Guid CategoryId { get; set; }
    public Category Category { get; set; }

    public Product(string name, Guid categoryId)
    {
        Name = name;
        CategoryId = categoryId;

        Valiate();
    }

    public void Valiate()
    {
        DomainExceptionValidation.When(string.IsNullOrEmpty(Name), RS.DATA_ANOTATION_REQUIRED.Replace("{0}", RS.PRODUCT_LBL_NAME));
        DomainExceptionValidation.When((CategoryId == null || CategoryId == Guid.Empty), RS.DATA_ANOTATION_REQUIRED.Replace("{0}", RS.PRODUCT_LBL_CATEGORY));
    }
}
