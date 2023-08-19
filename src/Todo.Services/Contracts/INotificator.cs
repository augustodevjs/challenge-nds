using Todo.Services.Notifications;

namespace Todo.Services.Contracts;

public interface INotificator
{
    bool TemNotificacao();
    List<Notification> ObterNotificacoes();
    void Handle(Notification notificacao);
}