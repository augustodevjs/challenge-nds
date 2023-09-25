namespace Todo.Domain.Contracts;

public interface IUnityOfWork
{
    Task<bool> Commit();
}