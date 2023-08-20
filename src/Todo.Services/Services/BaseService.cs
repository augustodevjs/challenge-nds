using FluentValidation;
using Todo.Domain.Models;
using Todo.Services.Contracts;
using FluentValidation.Results;
using Todo.Services.Notifications;

namespace Todo.Services.Services;

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