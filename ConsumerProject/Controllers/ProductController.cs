using Application.Common.Queries;
using Application.DTOs;
using Application.MediatR.UseCases;
using AutoMapper;
using Common.MediatR.Commands;
using Controllers.Base;
using Domain.Entities;
using Domain.Interfaces.Common;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConsumerProject.Controllers;

[ApiController]
[Route("produto")]
public class ProductController : MainController
{
    private readonly ICache _cache;

    public ProductController(IMediator mediator, IMapper mapper, ICache cache) : base(mediator, mapper)
    {
        _cache = cache;
    }

    /// <summary>
    /// Busca uma lista de produtos
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("listar-produtos")]
    [Queue("produtos")]
    public async Task<IActionResult> GetAllAsync()
    {
        if (await _cache.ExitsAsync("produtos"))
            return CustomResponse(await _cache.GetObjectAsync<IEnumerable<ProductDto>>("produtos"));
        else
        {
            var query = await _mediator.Send(new GetQuerablePredicated<Product>());
            var produtos = _mapper.Map<IEnumerable<ProductDto>>(query.Include(a => a.Category));

            if (produtos is not null)
                await _cache.AddToCacheAsync("produtos", produtos, 60);

            return CustomResponse(await _cache.GetObjectAsync<IEnumerable<ProductDto>>("produtos"));
        }
    }

    /// <summary>
    /// Busca um produto pelo id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("buscar-produto/{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        if (await _cache.ExitsAsync($"produto:{id}"))
            return CustomResponse(await _cache.GetObjectAsync<ProductDto>($"produto:{id}"));
        else
        {
            var query = await _mediator.Send(new GetQuerablePredicated<Product> { Predicated = a => a.Id == id });
            var produto = _mapper.Map<ProductDto>(await query.Include(a => a.Category).FirstOrDefaultAsync());
            if (produto is not null)
                await _cache.AddToCacheAsync($"produto:{produto.Id}", produto, 60);

            return CustomResponse(await _cache.GetObjectAsync<ProductDto>($"produto:{id}"));
        }
    }

    /// <summary>
    /// Deleta um produto
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("deletar-produto/{id}")]
    [Queue("produtos")]
    public async Task DeleteProductByIdAsync([FromRoute] Guid id)
        => await _mediator.Send(new DeleteProduct { Id = id });

    /// <summary>
    /// Agenda uma tarefa com Tags
    /// </summary>
    [HttpGet]
    [Route("tarefa-deletar-produto/{id}")]
    public async void ScheduleDeleteProductByIdAsync([FromRoute] Guid id)
        => await Task.FromResult(BackgroundJob.Schedule(() => DeleteProductByIdAsync(id), TimeSpan.FromMinutes(1)));
}
