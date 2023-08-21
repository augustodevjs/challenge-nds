using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Domain.Models;

namespace Todo.Infra.Data.Mappings;

public class AssignmentListMapping : IEntityTypeConfiguration<AssignmentList>
{
    public void Configure(EntityTypeBuilder<AssignmentList> builder)
    {
        builder.HasKey(c => c.Id);

        builder
            .Property(c => c.Name)
            .IsRequired();

        builder
            .Property(c => c.UserId)
            .IsRequired();

        builder
            .HasMany(c => c.Assignments)
            .WithOne(c => c.AssignmentList);

        builder.ToTable("Lista de Tarefas");
    }
}