using Application.Common.Queries;
using Application.DTOs;
using AutoMapper;
using Common.MediatR.Commands;
using Controllers.Base;
using Domain.Entities;
using Domain.Interfaces.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConsumerProject.Controllers;

[ApiController]
[Route("categoria")]
public class CategoryController : MainController
{
    private readonly ICache _cache;

    public CategoryController(IMediator mediator, IMapper mapper, ICache cache) : base(mediator, mapper)
    {
        _cache = cache;
    }

    /// <summary>
    /// Busca uma lista de categorias
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("listar-categorias")]
    public async Task<IActionResult> GetAllAsync()
    {
        if (await _cache.ExitsAsync("categorias"))
            return CustomResponse(await _cache.GetObjectAsync<IEnumerable<CategoryDto>>("categorias"));
        else
        {
            var query = await _mediator.Send(new GetQuerablePredicated<Category>());
            var categories = _mapper.Map<IEnumerable<CategoryDto>>(query);

            if (categories is not null)
                await _cache.AddToCacheAsync("categorias", categories, 60);

            return CustomResponse(await _cache.GetObjectAsync<IEnumerable<CategoryDto>>("categorias"));
        }
    }

    /// <summary>
    /// Busca um categoria pelo id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("buscar-categoria/{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        if (await _cache.ExitsAsync($"categoria:{id}"))
            return CustomResponse(await _cache.GetObjectAsync<CategoryDto>($"categoria:{id}"));
        else
        {
            var query = await _mediator.Send(new GetQuerablePredicated<Category> { Predicated = a => a.Id == id });
            var category = _mapper.Map<CategoryDto>(await query.FirstOrDefaultAsync());
            if (category is not null)
                await _cache.AddToCacheAsync($"categoria:{category.Id}", category, 60);

            return CustomResponse(await _cache.GetObjectAsync<CategoryDto>($"categoria:{id}"));
        }
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> Save([FromBody] Category entity)
        => CustomResponse(await _mediator.Send(new PutEntityCommand<Category> { Id = entity.Id, Entity = entity, UserName = "Anderson" }));
}
