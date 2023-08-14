namespace Todo.Domain.Models;

public abstract class Entity
{
    public Guid Id { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}