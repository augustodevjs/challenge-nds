using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Domain.Models;

namespace Todo.Infra.Data.Mappings;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(c => c.Id);

        builder
            .Property(c => c.Name)
            .IsRequired()
            .HasColumnType("VARCHAR(150)");

        builder
            .Property(c => c.Email)
            .IsRequired()
            .HasColumnType("VARCHAR(100)");

        builder
            .Property(c => c.Password)
            .IsRequired()
            .HasColumnType("VARCHAR(250)");

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
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId);

        builder
            .HasMany(c => c.AssignmentLists)
            .WithOne(c => c.User);
    }
}