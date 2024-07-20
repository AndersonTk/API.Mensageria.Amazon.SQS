using Consumer.Test.Common;
using Domain.Entities;

namespace Consumer.Test;

public class ProductFixture : BaseFixture
{
    public ProductFixture() : base()
    { }

    public Product NewProduct() => new Product(Faker.Commerce.ProductName(), Guid.NewGuid());
}

public class ProductFixtureCollection : ICollectionFixture<ProductFixture>
{ }
