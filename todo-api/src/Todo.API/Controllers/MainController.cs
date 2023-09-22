using Todo.API.Responses;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using Todo.Application.Notifications;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Todo.API.Controllers;

[ApiController]
public abstract class MainController : ControllerBase
{
    private readonly INotificator _notificator;
    
    protected MainController(INotificator notificator)
    {
        _notificator = notificator;
    }
    
    protected IActionResult NoContentResponse() 
        => CustomResponse(NoContent());

    protected IActionResult CreatedResponse(string uri = "", object? result = null) 
        => CustomResponse(Created(uri, result));

    protected IActionResult OkResponse(object? result = null) 
        => CustomResponse(Ok(result));
    
    protected IActionResult CustomResponse(IActionResult objectResult)
    {
        if (OperacaoValida)
        {
            return objectResult;
        }
        
        if (_notificator.IsNotFoundResource)
        {
            return NotFound();
        }

        var response = new BadRequestResponse(_notificator.GetNotifications().ToList());
        return BadRequest(response);
    }
    
    protected IActionResult CustomResponse(ModelStateDictionary modelState)
    {
        var erros = modelState.Values.SelectMany(e => e.Errors);
        foreach (var erro in erros)
        {
            AdicionarErroProcessamento(erro.ErrorMessage);
        }

        return CustomResponse(Ok(null));
    }

    protected IActionResult CustomResponse(ValidationResult validationResult)
    {
        foreach (var erro in validationResult.Errors)
        {
            AdicionarErroProcessamento(erro.ErrorMessage);
        }

        return CustomResponse(Ok(null));
    }
    
    private bool OperacaoValida => !(_notificator.HasNotification || _notificator.IsNotFoundResource);

    private void AdicionarErroProcessamento(string erro) => _notificator.Handle(erro);
}