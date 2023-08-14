namespace Todo.Core.Notifications;

public class Notification
{
    public string Mensagem { get; }

    public Notification(string mensagem)
    {
        Mensagem = mensagem;
    }
}