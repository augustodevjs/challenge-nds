using Todo.Core.Notifications;

namespace Todo.Core.Interfaces;

public interface INotificator
{
    bool TemNotificacao();
    List<Notification> ObterNotificacoes();
    void Handle(Notification notificacao);
}