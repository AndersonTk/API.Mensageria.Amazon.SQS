using Domain.Entities.Base;
using Domain.Validation;
using RS = Resources.Common;

namespace Domain.Entities;

public class Product : EntityBase
{
    public string Name { get; set; }

    public Product() { }

    public Product(string name)
    {
        Name = name;

        Valiate(name);
    }

    public void Valiate(string name)
    {
        DomainExceptionValidation.When(string.IsNullOrEmpty(name), RS.DATA_ANOTATION_REQUIRED.Replace("{0}", RS.PRODUCT_LBL_NAME));
    }
}
