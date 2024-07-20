using Application.DTOs;
using AutoMapper;
using Controllers.Base;
using Domain.Contracts;
using Domain.Entities;
using Domain.Interfaces.Common;
using Infra.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProducerProject.Controllers;

[ApiController]
[Route("categoria")]
public class CategoryController : MainController
{
    private readonly IEventBusInterface _bus;
    private readonly ProducerDbContext _context;

    public CategoryController(IMapper mapper,
                              IEventBusInterface bus,
                              ProducerDbContext context) : base(mapper)
    {
        _bus = bus;
        _context = context;
    }

    /// <summary>
    /// Envia uma mensagem sqs para fila de categorias
    /// </summary>
    /// <param name="contract"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("salvar")]
    public async Task<IActionResult> SendSave([FromBody] CategoryContract contract)
    {
        await _bus.PublishMessage(contract);
        return CustomResponse("SendMessage");
    }

    /// <summary>
    /// Lista as categorias
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetList()
    => CustomResponse(_mapper.Map<IList<CategoryDto>>(await _context.Category.ToListAsync()));

    /// <summary>
    /// Salva uma categoria
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("create-category")]
    public async Task<IActionResult> Create([FromBody] Category category)
    {
        var saved = await _context.Category.AddAsync(category);
        await _context.SaveChangesAsync();
        return CustomResponse(_mapper.Map<CategoryDto>(saved.Entity));
    }

    /// <summary>
    /// Atualiza uma categoria
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("update-category")]
    public async Task<IActionResult> Update([FromBody] Category category)
    {
        if (!await _context.Category.AnyAsync(a => a.Id == category.Id))
            throw new Exception($"{nameof(category.Id)} não existe");

        var saved = _context.Category.Update(category);
        await _context.SaveChangesAsync();

        return CustomResponse(_mapper.Map<CategoryDto>(saved.Entity));
    }

    /// <summary>
    /// Deleta uma categoria pelo id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("delete-category/{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var category = await _context.Category.FindAsync(id);

        if (category is null)
            throw new Exception($"{nameof(category)} não encontrado");

        _context.Category.Remove(category);
        await _context.SaveChangesAsync();

        return CustomResponse($"{category.Name} deletado");
    }
}
