using Domain.Entities;
namespace Consumer.Test.Domain.AlunoTest;

public class AlunoTest
{
    [Fact (DisplayName = nameof(Instantiable))]
    [Trait("Domain", "Aluno")]
    public void Instantiable()
    {
        var aluno = new Aluno();
    }
}