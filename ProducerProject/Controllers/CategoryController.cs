using Domain.Contracts;
using Domain.Interfaces.Common;
using Microsoft.AspNetCore.Mvc;

namespace ProducerProject.Controllers;

[ApiController]
[Route("categoria")]
public class CategoryController : ControllerBase
{
    private readonly IEventBusInterface<CategoryContract> _bus;

    public CategoryController(IEventBusInterface<CategoryContract> bus)
    {
        _bus = bus;
    }

    [HttpPost]
    [Route("salvar")]
    public async Task<IActionResult> SendSaveProduct(CategoryContract contract)
    {
        await _bus.PublishMessage(contract);
        return Ok();
    }
}
