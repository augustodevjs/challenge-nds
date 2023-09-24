using FluentAssertions;
using Todo.Domain.Models;

namespace Todo.Domain.Tests.Models;

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
        using (new AssertionScope())
        {
            assignment.Concluded.Should().BeTrue();
            assignment.ConcludedAt.Should().NotBeNull();
        }
    }

    [Fact]
    public void SetUnconcluded_MustUnsetConcludedAndConcludedA()
    {
        // Arrange
        var assignment = new Assignment();

        // Act
        assignment.SetUnconcluded();

        // Assert
        using (new AssertionScope())
        {
            assignment.Concluded.Should().BeFalse();
            assignment.ConcludedAt.Should().BeNull();
        }
    }
}