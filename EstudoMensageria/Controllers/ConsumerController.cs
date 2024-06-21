using Domain.Contracts;
using Domain.Interfaces;
using Domain.Interfaces.Common;
using Microsoft.AspNetCore.Mvc;

namespace ProducerProject.Controllers;

[ApiController]
[Route("Producer")]
public class ConsumerController : ControllerBase
{
    private readonly IEventBusInterface<AlunoContract> _bus;
    private readonly IAlunoRepository _repository;

    public ConsumerController(IEventBusInterface<AlunoContract> bus, IAlunoRepository repository)
    {
        _bus = bus;
        _repository = repository;
    }

    [HttpPost]
    [Route("aluno")]
    public async Task<IActionResult> SendStudentConsumer(AlunoContract contract)
    {
        await _bus.PublishMessage(contract);
        return Ok();
    }
}
