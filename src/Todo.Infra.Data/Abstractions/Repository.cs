using Microsoft.EntityFrameworkCore;
using Todo.Domain.Contracts.Repository;
using Todo.Domain.Models;
using Todo.Infra.Data.Context;

namespace Todo.Infra.Data.Abstractions;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
{
    protected readonly TodoDbContext Db;
    protected readonly DbSet<TEntity> DbSet;
    
    public Repository(TodoDbContext db)
    {
        Db = db;
        DbSet = db.Set<TEntity>();
    }

    public virtual async Task<TEntity?> GetById(Guid? id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual async Task<List<TEntity>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public virtual async Task Create(TEntity entity)
    {
        DbSet.Add(entity);
        await SaveChanges();
    }

    public virtual async Task Update(TEntity entity)
    {
        DbSet.Update(entity);
        await SaveChanges();
    }

    public virtual async Task Delete(Guid id)
    {
        DbSet.Remove(new TEntity { Id = id });
        await SaveChanges();
    }

    public async Task<int> SaveChanges()
    {
        return await Db.SaveChangesAsync();
    }
    
    public void Dispose()
    {
        Db?.Dispose();
    }
}