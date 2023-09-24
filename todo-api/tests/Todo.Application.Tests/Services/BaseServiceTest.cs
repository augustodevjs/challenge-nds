using Moq;
using Todo.Application.Notifications;

namespace Todo.Application.Tests.Services;

public abstract class BaseServiceTest
{
    protected  readonly Mock<INotificator> NotificatorMock = new();
    
    private readonly List<string> _erros = new();
    protected List<string> Erros => _erros.ToList();
    protected bool NotFound { get; private set; }

    protected BaseServiceTest()
    {
        NotificatorMock
            .Setup(c => c.Handle(It.IsAny<List<ValidationFailure>>()))
            .Callback<List<ValidationFailure>>(fails =>
            {
                fails.ForEach(error => _erros.Add(error.ErrorMessage));
            });

        NotificatorMock
            .Setup(c => c.Handle(It.IsAny<string>()))
            .Callback<string>(notification => _erros.Add(notification));
        
        NotificatorMock
            .Setup(c => c.HandleNotFoundResource())
            .Callback(() => NotFound = true);

        NotificatorMock
            .Setup(c => c.GetNotifications())
            .Returns(() => _erros);

        NotificatorMock
            .Setup(c => c.HasNotification)
            .Returns(() => Erros.Any());
    }
}