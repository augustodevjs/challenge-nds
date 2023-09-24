
using Todo.Application.Notifications;

namespace Todo.Application.Tests.Notifications;

public class NotificatorTest
{
    private readonly Faker _faker = new("pt_BR");
    private readonly Notificator _notificator = new();

    [Fact]
    [Trait("Notificator", "Handle")]
    public void Handle_PassingNotification_ContainsNotification()
    {
        // Arrange
        var message = _faker.Random.String();

        // Act
        _notificator.Handle(message);

        // Assert
        using (new AssertionScope())
        { 
            _notificator.HasNotification.Should().BeTrue();
            _notificator.IsNotFoundResource.Should().BeFalse();
            _notificator.GetNotifications().Should().Contain(message);
        }
    }

    [Fact]
    public void Handle_PassingValidationFailure_ContainsNotifications()
    {
        // Arrange
        var faker = new Faker<ValidationFailure>("pt_BR");
        faker.CustomInstantiator(f => new ValidationFailure(f.Random.String(), f.Random.String()));

        var validationFailures = faker.Generate(4);

        var messages = validationFailures.Select(c => c.ErrorMessage);

        // Act
        _notificator.Handle(validationFailures);

        // Assert
        using (new AssertionScope())
        {
            _notificator.HasNotification.Should().BeTrue();
            _notificator.IsNotFoundResource.Should().BeFalse();
            _notificator.GetNotifications().Should().HaveCount(4);
            _notificator.GetNotifications().Should().Equal(messages);
        }
    }
    
    [Fact]
    public void HandleNotFoundResource_ExecuteWithSuccess()
    {
        // Act
        _notificator.HandleNotFoundResource();
        
        // Assert
        using (new AssertionScope())
        {
            _notificator.HasNotification.Should().BeFalse();
            _notificator.IsNotFoundResource.Should().BeTrue();
            _notificator.GetNotifications().Should().BeEmpty();
        }
    }

    [Fact]
    public void Handle_AfterCallingNotFoundResource_ShouldThrowException()
    {
        // Arrange
        const string notification = "Erro";

        // Act
        _notificator.HandleNotFoundResource();
        var action = () => _notificator.Handle(notification);

        // Assert
        action
            .Should()
            .ThrowExactly<InvalidOperationException>()
            .WithMessage("Cannot call Handle when there are NotFoundResource!");
    }
    
    [Fact]
    public void HandleNotFoundResource_AfterCallingHandle_ShouldThrowException()
    {
        // Arrange
        const string notification = "Erro";

        // Act
        _notificator.Handle(notification);
        var action = () => _notificator.HandleNotFoundResource();
        
        // Assert
        action
            .Should()
            .ThrowExactly<InvalidOperationException>()
            .WithMessage("Cannot call HandleNotFoundResource when there are notifications!");
    }
}