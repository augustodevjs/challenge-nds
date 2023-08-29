using Todo.Application.Notifications;

namespace Todo.Application.Contracts;

public interface INotificator
{
    bool hasNotification();
    List<Notification> getNotification();
    void Handle(Notification notification);
}