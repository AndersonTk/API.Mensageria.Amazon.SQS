using Application.Common.MediatR.Queries;
using Application.DTOs;
using AutoMapper;
using Controllers.Base;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConsumerProject.Controllers;

[ApiController]
[Route("produto")]
public class ProductController : MainController
{
    public ProductController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
    {
    }

    [HttpGet]
    [Route("listar-produtos")]
    public async Task<IActionResult> GetAllAsync()
        => CustomResponse(_mapper.Map<IEnumerable<ProductDto>>(await _mediator.Send(new GetAllQuery<Product>())));

    [HttpGet]
    [Route("buscar-produto/{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
        => CustomResponse(_mapper.Map<ProductDto>(_mapper.Map<ProductDto>(await _mediator.Send(new GetByIdQuery<Product> { Id = id }))));
}
