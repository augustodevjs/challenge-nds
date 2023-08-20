using Todo.Services.Contracts;

namespace Todo.Services.Notifications;

public class Notificator : INotificator
{
    private List<Notification> _notifications = new();

    public void Handle(Notification notification)
    {
        _notifications.Add(notification);
    }

    public List<Notification> getNotification()
    {
        return _notifications;
    }

    public bool hasNotification()
    {
        return _notifications.Any();
    }
}