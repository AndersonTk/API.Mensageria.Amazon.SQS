using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConsumerProject.Controllers;

[ApiController]
[Route("Consumer")]
public class AlunoController : ControllerBase
{
    private readonly IAlunoRepository _repository;

    public AlunoController(IAlunoRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [Route("aluno")]
    public async Task<IActionResult> GetAllAsync()
        => Ok(await _repository.GetAllAsync());

    [HttpGet]
    [Route("aluno/{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
        => Ok(await _repository.GetByIdAsync(id));

    [HttpPost]
    [Route("salvar-aluno")]
    public async Task<IActionResult> Save([FromBody] Aluno entity)
    {
        var aluno = await _repository.GetByIdAsNoTrackingAsync(entity.Id);

        if(aluno is null)
            return Ok(await _repository.AddAsync(entity));
        else
            return Ok(await _repository.UpdateAsync(entity));
    }
}
