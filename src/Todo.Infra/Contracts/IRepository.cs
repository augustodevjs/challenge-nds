using Todo.Domain.Models;

namespace Todo.Infra.Contracts;

public interface IRepository<TEntity> : IDisposable where TEntity : Entity
{
    Task Create(TEntity entity);
    Task<TEntity?> GetById(string? id);
    Task<List<TEntity>> GetAll();
    Task Update(TEntity entity);
    Task Delete(string id);
    Task<int> SaveChanges();
}