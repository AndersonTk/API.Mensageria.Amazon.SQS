using Bogus;
using Consumer.Test.Common;
using Domain.Entities;

namespace Consumer.Test;
public class CategoryFixture : BaseFixture
{
    public CategoryFixture() : base()
    { }

    public Category NewCategory() => new Category(Faker.Commerce.Categories(1)[0]);
}

public class CategoryFixtureCollection : ICollectionFixture<CategoryFixture>
{ }