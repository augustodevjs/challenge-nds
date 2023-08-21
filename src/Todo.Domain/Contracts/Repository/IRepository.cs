using Todo.Domain.Models;

namespace Todo.Domain.Contracts.Repository;

public interface IRepository<TEntity> : IDisposable where TEntity : Entity
{
    Task Create(TEntity entity);
    Task<TEntity?> GetById(Guid? id);
    Task<List<TEntity>> GetAll();
    Task Update(TEntity entity);
    Task Delete(Guid id);
    Task<int> SaveChanges();
}