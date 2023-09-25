using Todo.Domain.Models;
using System.Linq.Expressions;

namespace Todo.Domain.Contracts.Repository;

public interface IRepository<T> : IDisposable where T : Entity
{
    public IUnityOfWork UnityOfWork { get; }

    public Task<T?> FirstOrDefault(Expression<Func<T, bool>> expression);
    void Create(T entity);
    Task<T?> GetById(int? id);
    Task<List<T>> GetAll();
    void Update(T entity);
    void Delete(T entity);
}