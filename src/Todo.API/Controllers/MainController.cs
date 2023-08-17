using Todo.Core.Interfaces;
using Todo.Core.Notifications;
using Microsoft.AspNetCore.Mvc;
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

    protected bool OperacaoValida()
    {
        return !_notificador.TemNotificacao();
    }

    protected ActionResult CustomResponse(object? result = null)
    {
        if (OperacaoValida())
        {
            return Ok(result);
        }

        return BadRequest(new BadRequestResponse
        {
            Erros = _notificador.ObterNotificacoes().Select(n => n.Mensagem).ToList()
        });
    }

    protected ActionResult CustomResponse(ModelStateDictionary modelState)
    {
        if (!modelState.IsValid) NotificarErroModelInvalida(modelState);
        return CustomResponse();
    }

    protected void NotificarErroModelInvalida(ModelStateDictionary modelState)
    {
        var erros = modelState.Values.SelectMany(e => e.Errors);
        foreach (var erro in erros)
        {
            var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
            NotificarErro(errorMsg);
        }
    }

    protected void NotificarErro(string mensagem)
    {
        _notificador.Handle(new Notification(mensagem));
    }
}