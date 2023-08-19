using Todo.Services.Contracts;

namespace Todo.Services.Notifications;

public class Notificator : INotificator
{
    private List<Notification> _notificacoes = new();

    public void Handle(Notification notificacao)
    {
        _notificacoes.Add(notificacao);
    }

    public List<Notification> ObterNotificacoes()
    {
        return _notificacoes;
    }

    public bool TemNotificacao()
    {
        return _notificacoes.Any();
    }
}