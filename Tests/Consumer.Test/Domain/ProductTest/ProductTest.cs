namespace Consumer.Test;

public class ProductTest
{
    [Fact(DisplayName = nameof(Instantiable))]
    [Trait("Domain", "Product")]
    public void Instantiable()
    {
        var product = new ProductFixture().NewProduct();
    }
}
