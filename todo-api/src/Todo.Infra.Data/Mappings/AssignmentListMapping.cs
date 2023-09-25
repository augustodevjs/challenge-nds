using Todo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Todo.Infra.Data.Mappings;

public class AssignmentListMapping : IEntityTypeConfiguration<AssignmentList>
{
    public void Configure(EntityTypeBuilder<AssignmentList> builder)
    {
        builder
            .Property(c => c.Name)
            .IsRequired();

        builder
            .Property(c => c.UserId)
            .IsRequired();

        builder
            .Property(c => c.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasColumnType("DATETIME");
        
        builder
            .Property(c => c.UpdatedAt)
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnType("DATETIME");
        
        builder
            .HasMany(c => c.Assignments)
            .WithOne(c => c.AssignmentList);
    }
}