using Consumer.Test.Common;
using Domain.Entities;
using Domain.Validation;
using Microsoft.EntityFrameworkCore;
using RS = Resources.Common;

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
    
    [Fact(DisplayName = "Atualizar Produto")]
    [Trait("Domain", "Product")]
    public async Task UpdateProductAsync()
    {
        var newName = new ProductFixture().NewProduct().Name;
        var product = _unitOfWork.Products.GetAllQuerable().AsNoTracking().FirstOrDefault();

        if (product is null)
            throw new Exception("nenhum produto encontrado");

        var newProduct = new Product(newName, product.CategoryId);
        newProduct.Id = product.Id;

        var save = await _unitOfWork.Products.UpdateAsync(newProduct);

        Assert.NotNull(product);
        Assert.NotEmpty(product.Name);
        Assert.NotEqual(product.Name, save.Name);
        Assert.NotEqual(product.CategoryId, Guid.Empty);
        Assert.NotNull(save);
    }
    
    [Fact(DisplayName = "Erro ao criar Produto sem nome")]
    [Trait("Domain", "Product")]
    public async Task ErrorAddProductNoNameAsync()
    {
        var category = _unitOfWork.Categories.GetAllQuerable().FirstOrDefault();
        if (category is null)
            throw new Exception("nenhuma categoria encontrada");

        var product = new Product();
        product.CategoryId = category.Id;

        var save = await _unitOfWork.Products.AddAsync(product);

        var exception = Assert.Throws<DomainExceptionValidation>(() => save.Validate());

        Assert.NotNull(product);
        Assert.Null(product.Name);
        Assert.Equal(RS.DATA_ANOTATION_REQUIRED.Replace("{0}", RS.PRODUCT_LBL_NAME), exception.Message);
        Assert.NotEqual(product.CategoryId, Guid.Empty);
        Assert.NotNull(save);
    }
    
    [Fact(DisplayName = "Erro ao criar Produto sem categoria")]
    [Trait("Domain", "Product")]
    public async Task ErrorAddProductNoCategoryAsync()
    {
        var newName = new ProductFixture().NewProduct().Name;

        var product = new Product();
        product.Name = newName;

        var save = await _unitOfWork.Products.AddAsync(product);

        var exception = Assert.Throws<DomainExceptionValidation>(() => product.Validate());

        Assert.NotNull(product);
        Assert.NotNull(product.Name);
        Assert.Equal(RS.DATA_ANOTATION_REQUIRED.Replace("{0}", RS.PRODUCT_LBL_CATEGORY), exception.Message);
        Assert.NotNull(save);
    }
}
