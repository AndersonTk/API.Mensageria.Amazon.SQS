using AutoMapper;
using Domain.Entities.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Controllers.Base;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class MainController : ControllerBase
{
    public readonly IMediator _mediator;
    public readonly IMapper _mapper;

    public MainController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    protected ICollection<string> Erros = new List<string>();
    protected IActionResult CustomResponse<T>(T result)
    {
        if (ValidOperation())
            return Ok(new ResultResponse<T> { success = true, type = "success", message = "", Data = result });

        return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
        {
            {"Mensagens", Erros.ToArray() }
        }));
    }

    protected IActionResult CustomResponse(ModelStateDictionary modelState)
    {
        var erros = modelState.Values.SelectMany(e => e.Errors);

        foreach (var erro in erros)
        {
            AddErrorsProcess(erro.ErrorMessage);
        }

        return CustomResponse(modelState);
    }

    protected bool ValidOperation()
    {
        return !Erros.Any();
    }

    protected void AddErrorsProcess(string erro)
    {
        Erros.Add(erro);
    }

    protected void CleanErrosProccess()
    {
        Erros.Clear();
    }
}
