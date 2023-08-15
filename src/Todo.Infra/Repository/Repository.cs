using Microsoft.EntityFrameworkCore;
using Todo.Domain.Models;
using Todo.Infra.Context;
using Todo.Infra.Interfaces;

namespace Todo.Infra.Repository;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
{
    protected readonly TodoDbContext Db;
    protected readonly DbSet<TEntity> DbSet;
    
    public Repository(TodoDbContext db)
    {
        Db = db;
        DbSet = db.Set<TEntity>();
    }
    
    public Task Add(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<TEntity>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task Update(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<int> SaveChanges()
    {
        throw new NotImplementedException();
    }
    
    
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}