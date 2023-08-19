using FluentValidation;
using Todo.Domain.Models;
using FluentValidation.Results;
using Todo.Services.Contracts;
using Todo.Services.Notifications;

namespace Todo.Services.Services;

public abstract class BaseService
{
    private readonly INotificator _notificator;

    protected BaseService(INotificator notificator)
    {
        _notificator = notificator;
    }

    protected void Notificar(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
        {
            Notificar(error.ErrorMessage);
        }
    }
    
    protected void Notificar(string mensagem)
    {
        _notificator.Handle(new Notification(mensagem));
    }
    
    protected bool ExecutarValidacao<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE> where TE : Entity
    {
        var validator = validacao.Validate(entidade);

        if(validator.IsValid) return true;

        Notificar(validator);

        return false;
    }
}