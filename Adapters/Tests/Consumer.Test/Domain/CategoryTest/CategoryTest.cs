using Consumer.Test.Common;
using Domain.Entities;
using Domain.Validation;
using Microsoft.EntityFrameworkCore;
using RS = Resources.Common;

namespace Consumer.Test;
public class CategoryTest : IClassFixture<UnitOfWorkFixture>
{
    private readonly UnitOfWorkFixture _unitOfWork;

    public CategoryTest(UnitOfWorkFixture unitOfWorkFixture)
    {
        _unitOfWork = unitOfWorkFixture;
    }

    [Fact(DisplayName = "Adicionar Categoria")]
    [Trait("Domain", "Category")]
    public async Task AddCategoryAsync()
    {
        var newName = new CategoryFixture().NewCategory().Name;

        var category = new Category(newName);

        var save = await _unitOfWork.Categories.AddAsync(category);

        Assert.NotNull(category);
        Assert.NotEmpty(category.Name);
        Assert.NotNull(save);
    }

    [Fact(DisplayName = "Atualizar Categoria")]
    [Trait("Domain", "Category")]
    public async Task UpdateCategoryAsync()
    {
        var newName = new CategoryFixture().NewCategory().Name;
        var category = _unitOfWork.Categories.GetAllQuerable().AsNoTracking().FirstOrDefault();

        if (category is null)
            throw new Exception("nenhum produto encontrado");

        var newCategory = new Category(newName);
        newCategory.Id = category.Id;

        var save = await _unitOfWork.Categories.UpdateAsync(newCategory);

        Assert.NotNull(category);
        Assert.NotEmpty(category.Name);
        Assert.NotEqual(category.Name, save.Name);
        Assert.NotNull(save);
    }

    [Fact(DisplayName = "Erro ao criar Categoria sem nome")]
    [Trait("Domain", "Category")]
    public async Task ErrorAddCategoryNoNameAsync()
    {
        var newCategory = new Category();

        var save = await _unitOfWork.Categories.AddAsync(newCategory);

        var exception = Assert.Throws<DomainExceptionValidation>(() => save.Validate());

        Assert.NotNull(newCategory);
        Assert.Null(newCategory.Name);
        Assert.Equal(RS.DATA_ANOTATION_REQUIRED.Replace("{0}", RS.CATEGORY_LBL_NAME), exception.Message);
        Assert.NotNull(save);
    }
}
