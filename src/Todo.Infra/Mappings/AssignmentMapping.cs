using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Domain.Models;

namespace Todo.Infra.Mappings;

public class AssignmentMapping : IEntityTypeConfiguration<Assignment>
{
    public void Configure(EntityTypeBuilder<Assignment> builder)
    {
        builder.HasKey(c => c.Id);

        builder
            .Property(c => c.Description)
            .IsRequired()
            .HasColumnType("VARCHAR(255)");

        builder
            .Property(c => c.UserId)
            .IsRequired();

        builder
            .Property(c => c.AssignmentListId)
            .IsRequired(false);

        builder
            .Property(c => c.Deadline)
            .HasColumnType("DATETIME")
            .IsRequired();

        builder
            .Property(c => c.Concluded)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnType("TINYINT");

        builder
            .Property(c => c.ConcludedAt)
            .HasColumnType("DATETIME")
            .IsRequired(false);

        builder
            .Property(c => c.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasColumnType("DATETIME");

        builder
            .Property(c => c.UpdatedAt)
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnType("DATETIME");

        builder.ToTable("Tarefas");
    }
}