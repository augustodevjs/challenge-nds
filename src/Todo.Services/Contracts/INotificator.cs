using Todo.Services.Notifications;

namespace Todo.Services.Contracts;

public interface INotificator
{
    bool hasNotification();
    List<Notification> getNotification();
    void Handle(Notification notification);
}