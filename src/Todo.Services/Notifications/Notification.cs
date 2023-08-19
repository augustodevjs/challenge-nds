namespace Todo.Services.Notifications;

public class Notification
{
    public string Mensagem { get; }

    public Notification(string mensagem)
    {
        Mensagem = mensagem;
    }
}