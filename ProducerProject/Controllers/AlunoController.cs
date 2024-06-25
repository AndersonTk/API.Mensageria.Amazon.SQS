using Domain.Contracts;
using Domain.Interfaces.Common;
using Microsoft.AspNetCore.Mvc;

namespace ProducerProject.Controllers;

[ApiController]
[Route("aluno")]
public class AlunoController : ControllerBase
{
    private readonly IEventBusInterface<AlunoContract> _bus;

    public AlunoController(IEventBusInterface<AlunoContract> bus)
    {
        _bus = bus;
    }

    [HttpPost]
    [Route("salvar")]
    public async Task<IActionResult> SendSaveAluno(AlunoContract contract)
    {
        await _bus.PublishMessage(contract);
        return Ok();
    }
}
