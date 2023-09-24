using Todo.Domain.Models;
using Todo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Todo.Domain.Contracts.Repository;

namespace Todo.Infra.Data.Abstractions;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    protected readonly TodoDbContext Db;
    protected readonly DbSet<TEntity> DbSet;

    public Repository(TodoDbContext db)
    {
        Db = db;
        DbSet = db.Set<TEntity>();
    }

    public virtual async Task<List<TEntity>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public virtual async Task<TEntity?> GetById(int? id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual async Task Create(TEntity entity)
    {
        DbSet.Add(entity);
        await SaveChangesAsync();
    }

    public virtual async Task Update(TEntity entity)
    {
        DbSet.Update(entity);
        await SaveChangesAsync();
    }

    public virtual async Task Delete(TEntity entity)
    {
        DbSet.Remove(entity);
        await SaveChangesAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await Db.SaveChangesAsync();
    }

    public void Dispose()
    {
        Db?.Dispose();
    }
}