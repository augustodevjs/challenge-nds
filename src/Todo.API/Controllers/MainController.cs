using Microsoft.AspNetCore.Mvc;
using Todo.Application.Contracts;
using Todo.Application.Notifications;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Todo.API.Controllers;

public sealed class BadRequestResponse
{
    public List<string> Erros { get; set; } = new();
}

[ApiController]
public abstract class MainController : ControllerBase
{
    private readonly INotificator _notificador;

    protected MainController(INotificator notificador)
    {
        _notificador = notificador;
    }

    protected bool ValidOperation()
    {
        return !_notificador.hasNotification();
    }

    protected ActionResult CustomResponse(object? result = null)
    {
        if (ValidOperation())
        {
            return Ok(result);
        }

        return BadRequest(new BadRequestResponse
        {
            Erros = _notificador.getNotification().Select(n => n.Message).ToList()
        });
    }

    protected ActionResult CustomResponse(ModelStateDictionary modelState)
    {
        if (!modelState.IsValid) NotifyErrorModelInvalida(modelState);
        return CustomResponse();
    }

    protected void NotifyErrorModelInvalida(ModelStateDictionary modelState)
    {
        var erros = modelState.Values.SelectMany(e => e.Errors);
        foreach (var erro in erros)
        {
            var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
            NotifyError(errorMsg);
        }
    }

    protected void NotifyError(string message)
    {
        _notificador.Handle(new Notification(message));
    }
}