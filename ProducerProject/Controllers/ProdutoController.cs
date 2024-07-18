using Domain.Contracts;
using Domain.Interfaces.Common;
using Microsoft.AspNetCore.Mvc;
using SignalR.Hub;

namespace ProducerProject.Controllers;

[ApiController]
[Route("produto")]
public class ProdutoController : ControllerBase
{
    private readonly IEventBusInterface<ProductContract> _bus;
    private readonly SignalRClient _signalRClient;

    public ProdutoController(IEventBusInterface<ProductContract> bus, SignalRClient signalRClient)
    {
        _bus = bus;
        _signalRClient = signalRClient;
    }

    [HttpPost]
    [Route("salvar")]
    public async Task<IActionResult> SendSaveProduct(ProductContract contract)
    {
        await _bus.PublishMessage(contract);
        return Ok();
    }

    [HttpGet]
    [Route("SendProduct")]
    public async Task<IActionResult> SendProducts()
    {
        await _signalRClient.StartAsync();
        await _signalRClient.GetListProductsAsync();
        //await _signalRClient.StopAsync();
        return Ok();
    }
}
