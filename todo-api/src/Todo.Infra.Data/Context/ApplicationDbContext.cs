using System.Reflection;
using Todo.Domain.Models;
using Todo.Domain.Contracts;
using FluentValidation.Results;
using Todo.Infra.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Todo.Infra.Data.Context;

public class ApplicationDbContext : DbContext, IUnityOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Assignment> Assignments { get; set; } = null!;
    public DbSet<AssignmentList> AssignmentLists { get; set; } = null!;

    public async Task<bool> Commit() => await SaveChangesAsync() > 0;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        ApplyConfigurations(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private static void ApplyConfigurations(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<ValidationResult>();
        modelBuilder.ApplyEntityConfiguration();
    }
}