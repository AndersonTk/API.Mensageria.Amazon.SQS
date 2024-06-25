using Bogus;

namespace Consumer.Test.Common;

public abstract class BaseFixture
{
    public BaseFixture()
        => Faker = new Faker("pt_BR");

    public Faker Faker { get; set; }
}
