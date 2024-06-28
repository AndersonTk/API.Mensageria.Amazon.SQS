using Application.Common.MediatR.Queries;
using Application.Common.Queries;
using Application.DTOs;
using AutoMapper;
using Controllers.Base;
using Domain.Entities;
using Hangfire;
using Hangfire.Tags;
using Hangfire.Tags.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConsumerProject.Controllers;

[ApiController]
[Route("produto")]
public class ProductController : MainController
{
    public ProductController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
    { }

    [HttpGet]
    [Route("listar-produtos")]
    [Queue("produtos")]
    [Tag("produtos")]
    public async Task<IActionResult> GetAllAsync()
    {
        var query = await _mediator.Send(new GetQuerablePredicated<Product>());
        return CustomResponse(_mapper.Map<IEnumerable<ProductDto>>(query.Include(a => a.Category)));
    }

    [HttpGet]
    [Route("buscar-produto/{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
        => CustomResponse(_mapper.Map<ProductDto>(_mapper.Map<ProductDto>(await _mediator.Send(new GetByIdQuery<Product> { Id = id }))));

    /// <summary>
    /// Agenda uma tarefa com Tags
    /// </summary>
    [HttpGet]
    [Route("agendar-tarefa")]
    public void ScheduleAndAddTags()
    {
        BackgroundJob.Schedule(() => GetAllAsync(), TimeSpan.FromMinutes(1))
                     .AddTags("consumer")
                     .AddTags(nameof(Product).ToLower());
    }
}
