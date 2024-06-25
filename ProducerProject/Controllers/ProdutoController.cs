using Domain.Contracts;
using Domain.Interfaces.Common;
using Microsoft.AspNetCore.Mvc;

namespace ProducerProject.Controllers;

[ApiController]
[Route("produto")]
public class ProdutoController : ControllerBase
{
    private readonly IEventBusInterface<ProductContract> _bus;

    public ProdutoController(IEventBusInterface<ProductContract> bus)
    {
        _bus = bus;
    }

    [HttpPost]
    [Route("salvar")]
    public async Task<IActionResult> SendSaveProduct(ProductContract contract)
    {
        await _bus.PublishMessage(contract);
        return Ok();
    }
}
