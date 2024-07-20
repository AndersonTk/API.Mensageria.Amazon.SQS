using Application.DTOs;
using AutoMapper;
using Controllers.Base;
using Domain.Contracts;
using Domain.Entities;
using Domain.Interfaces.Common;
using Infra.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalR.Hub;

namespace ProducerProject.Controllers;

[ApiController]
[Route("produto")]
public class ProdutoController : MainController
{
    private readonly IEventBusInterface _bus;
    private readonly SignalRClient _signalRClient;
    private readonly ProducerDbContext _context;

    public ProdutoController(IMapper mapper,
                             IEventBusInterface bus,
                             SignalRClient signalRClient,
                             ProducerDbContext context) : base(mapper)
    {
        _bus = bus;
        _signalRClient = signalRClient;
        _context = context;
    }


    /// <summary>
    /// Envia uma mensagem sqs para fila de produdos
    /// </summary>
    /// <param name="contract"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("send-sqs-product")]
    public async Task<IActionResult> SendSave([FromBody] ProductContract contract)
    {
        await _bus.PublishMessage(contract);
        return CustomResponse("SendMessage");
    }

    /// <summary>
    /// Lista os produtos
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetList()
        => CustomResponse(_mapper.Map< IList<ProductDto>>(await _context.Product.ToListAsync()));

    /// <summary>
    /// Salva um produto
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("create-product")]
    public async Task<IActionResult> Create([FromBody] Product product)
    {
        var saved = await _context.Product.AddAsync(product);
        await _context.SaveChangesAsync();
        return CustomResponse(_mapper.Map<ProductDto>(saved.Entity));
    }

    /// <summary>
    /// Atualiza um produto
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("update-product")]
    public async Task<IActionResult> Update([FromBody] Product product)
    {
        if (!await _context.Product.AnyAsync(a => a.Id == product.Id))
            throw new Exception($"{nameof(product.Id)} não existe");

        var saved = _context.Product.Update(product);
        await _context.SaveChangesAsync();

        return CustomResponse(_mapper.Map<ProductDto>(saved.Entity));
    }

    /// <summary>
    /// Deleta um produto pelo id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("delete-product/{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var product = await _context.Product.FindAsync(id);

        if (product is null)
            throw new Exception($"{nameof(product)} não encontrado");

        _context.Product.Remove(product);
        await _context.SaveChangesAsync();

        return CustomResponse($"{product.Name} deletado");
    }

    //[HttpGet]
    //[Route("SendProduct")]
    //public async Task<IActionResult> SendProducts()
    //{
    //    await _signalRClient.StartAsync();
    //    await _signalRClient.GetListProductsAsync();
    //    //await _signalRClient.StopAsync();
    //    return Ok();
    //}
}
