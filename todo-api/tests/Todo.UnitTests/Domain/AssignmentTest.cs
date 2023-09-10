using Todo.Domain.Models;

namespace Todo.UnitTests.Domain;

public class AssignmentTest
{
    [Fact]
    public void SetConcluded_MustSetConcludedAndConcludedAt()
    {
        // Arrange
        var assignment = new Assignment();

        // Act
        assignment.SetConcluded();

        // Assert
        Assert.True(assignment.Concluded);
        Assert.NotNull(assignment.ConcludedAt);
    }

    [Fact]
    public void SetUnconcluded_MustUnsetConcludedAndConcludedA()
    {
        // Arrange
        var assignment = new Assignment();

        // Act
        assignment.SetUnconcluded();

        // Assert
        Assert.False(assignment.Concluded);
        Assert.Null(assignment.ConcludedAt);
    }
}