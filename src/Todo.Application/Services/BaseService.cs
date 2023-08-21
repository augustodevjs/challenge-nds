using FluentValidation;
using Todo.Domain.Models;
using FluentValidation.Results;
using Todo.Application.Contracts;
using Todo.Application.Notifications;

namespace Todo.Application.Services;

public abstract class BaseService
{
    private readonly INotificator _notificator;

    protected BaseService(INotificator notificator)
    {
        _notificator = notificator;
    }
    
    protected void Notify(string message)
    {
        _notificator.Handle(new Notification(message));
    }
    
    protected void Notify(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
        {
            Notify(error.ErrorMessage);
        }
    }
    
    protected bool ExecuteValidation<TV, TE>(TV validation, TE entity) where TV : AbstractValidator<TE> where TE : Entity
    {
        var validator = validation.Validate(entity);

        if(validator.IsValid) return true;

        Notify(validator);

        return false;
    }
}