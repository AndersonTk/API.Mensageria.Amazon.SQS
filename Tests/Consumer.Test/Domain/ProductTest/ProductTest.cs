using Consumer.Test.Common;
using Domain.Entities;

namespace Consumer.Test;

public class ProductTest : IClassFixture<UnitOfWorkFixture>
{
    private readonly UnitOfWorkFixture _unitOfWork;

    public ProductTest(UnitOfWorkFixture unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [Fact(DisplayName = "Adicionar Produto")]
    [Trait("Domain", "Product")]
    public async Task AddProductAsync()
    {
        var productName = new ProductFixture().NewProduct().Name;
        var category = _unitOfWork.Categories.GetAllQuerable().FirstOrDefault();

        if (category is null)
            throw new Exception("nenhuma categoria encontrada");

        var product = new Product(productName, category.Id);

        var save = await _unitOfWork.Products.AddAsync(product);

        Assert.NotNull(product);
        Assert.NotEmpty(product.Name);
        Assert.NotEqual(product.CategoryId, Guid.Empty);
        Assert.NotNull(save);
    }
}
