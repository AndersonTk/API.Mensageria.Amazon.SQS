using Domain.Contracts;
using Domain.Interfaces;
using Domain.Interfaces.Common;
using Microsoft.AspNetCore.Mvc;

namespace ProducerProject.Controllers;

[ApiController]
[Route("Producer")]
public class ProducerController : ControllerBase
{
    private readonly IEventBusInterface<AlunoContract> _bus;

    public ProducerController(IEventBusInterface<AlunoContract> bus)
    {
        _bus = bus;
    }

    [HttpPost]
    [Route("aluno")]
    public async Task<IActionResult> SendStudentConsumer(AlunoContract contract)
    {
        await _bus.PublishMessage(contract);
        return Ok();
    }
}
