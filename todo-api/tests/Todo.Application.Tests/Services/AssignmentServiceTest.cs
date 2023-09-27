using Todo.Domain.Models;
using Todo.Domain.Filter;
using Todo.Infra.Data.Paged;
using System.Security.Claims;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Todo.Application.Services;
using Todo.Application.Tests.Helper;
using Todo.Application.Tests.Fixtures;
using Todo.Domain.Contracts.Repository;
using Todo.Application.DTO.V1.ViewModel;
using Todo.Application.DTO.V1.InputModel;

namespace Todo.Application.Tests.Services;

public class AssignmentServiceTest : BaseServiceTest, IClassFixture<ServicesFixtures>
{
    private readonly AssignmentService _assignmentService;
    private readonly Mock<IAssignmentRepository> _assignmentRepositoryMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;

    public AssignmentServiceTest(ServicesFixtures servicesFixtures)
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _assignmentRepositoryMock = new Mock<IAssignmentRepository>();
        _assignmentService = new AssignmentService(
            servicesFixtures.Mapper,
            NotificatorMock.Object,
            _httpContextAccessorMock.Object,
            _assignmentRepositoryMock.Object
        );
    }

    [Fact]
    public async Task Search_ReturnPagedViewModelOfAssignmentViewModel()
    {
        // Arrange
        SetupMockHttpContext();

        var assignmentSearch = new AssignmentSearchInputModel
        {
            PerPage = 1,
            Page = 1
        };

        _assignmentRepositoryMock
            .Setup(c => c.Search(1, It.IsAny<AssignmentFilter>(), assignmentSearch.PerPage, assignmentSearch.Page,
                null)).ReturnsAsync(new PagedResult<Assignment>
            {
                Items = new List<Assignment>()
            });

        // Act
        var search = await _assignmentService.Search(assignmentSearch);

        // Assert

        using (new AssertionScope())
        {
            search.Should().BeOfType<PagedViewModel<AssignmentViewModel>>();
            _assignmentRepositoryMock.Verify(c => c.Search(1, It.IsAny<AssignmentFilter>(), assignmentSearch.PerPage,
                assignmentSearch.Page,
                null), Times.Once);
        }
    }

    [Fact]
    public async Task GetById_AssignmentExistent_ReturnAssignment()
    {
        // Arrange
        const int id = 1;
        SetupMockHttpContext();

        var assignment = new Assignment { Id = id };
        _assignmentRepositoryMock.Setup(c => c.GetById(id, 1)).ReturnsAsync(assignment);

        // Act
        var assignmentService = await _assignmentService.GetById(1);

        // Assert
        using (new AssertionScope())
        {
            NotFound.Should().BeFalse();
            assignmentService.Should().NotBeNull();
            assignmentService.Should().BeOfType<AssignmentViewModel>();
            NotificatorMock.Verify(c => c.HandleNotFoundResource(), Times.Never);
            _assignmentRepositoryMock.Verify(c => c.GetById(1, 1), Times.Once);
        }
    }

    [Fact]
    public async Task GetById_AssignmentNotExistent_ReturnNotFoundResource()
    {
        // Arrange
        SetupMockHttpContext();

        _assignmentRepositoryMock.Setup(c => c.GetById(1, 1)).ReturnsAsync(null as Assignment);

        // Act
        var assignment = await _assignmentService.GetById(1);

        // Assert
        using (new AssertionScope())
        {
            assignment.Should().BeNull();
            NotFound.Should().BeTrue();
            NotificatorMock.Verify(c => c.HandleNotFoundResource(), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.GetById(1, 1), Times.Once);
        }
    }

    [Fact]
    public async Task Create_Assignment_ReturnAssignmentViewModel()
    {
        // Arrange
        _assignmentRepositoryMock.Setup(c => c.FirstOrDefault(It.IsAny<Expression<Func<Assignment, bool>>>()))
            .ReturnsAsync(null as Assignment);
        _assignmentRepositoryMock.Setup(c => c.UnityOfWork.Commit()).ReturnsAsync(true);

        var assignmentInputModel = new AddAssignmentInputModel
        {
            Description = "Teste"
        };

        // Act
        var assignment = await _assignmentService.Create(assignmentInputModel);

        // Assert
        using (new AssertionScope())
        {
            assignment.Should().NotBeNull();
            Erros.Should().BeEmpty();
            _assignmentRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Once);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Never);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
        }
    }

    [Fact]
    public async Task Create_Assignment_HandleErrorValidation()
    {
        // Arrange
        _assignmentRepositoryMock.Setup(c => c.FirstOrDefault(It.IsAny<Expression<Func<Assignment, bool>>>()))
            .ReturnsAsync(null as Assignment);
        var assignmentInputModel = new AddAssignmentInputModel();

        // Act
        var assignment = await _assignmentService.Create(assignmentInputModel);

        // Assert
        using (new AssertionScope())
        {
            assignment.Should().BeNull();
            Erros.Should().NotBeEmpty();
            Erros.Should().Contain("O campo de descrição não pode ser deixado vazio.");
            NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Once);
        }
    }

    [Fact]
    public async Task Create_Assignment_HandleErrorWhenInfoAlreadyExist()
    {
        // Arrange
        var assignment = new Assignment();

        _assignmentRepositoryMock.Setup(c => c.FirstOrDefault(It.IsAny<Expression<Func<Assignment, bool>>>()))
            .ReturnsAsync(assignment);
        var assignmentInputModel = new AddAssignmentInputModel();

        // Act
        var assignmentService = await _assignmentService.Create(assignmentInputModel);

        // Assert
        using (new AssertionScope())
        {
            assignmentService.Should().BeNull();
            Erros.Should().NotBeEmpty();
            Erros.Should().Contain("Já existe uma tarefa cadastrada com essas informações");
            NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Once);
        }
    }

    [Fact]
    public async Task Create_Assignment_HandleErrorUnityOfWorkCommit()
    {
        // Arrange
        _assignmentRepositoryMock.Setup(c => c.FirstOrDefault(It.IsAny<Expression<Func<Assignment, bool>>>()))
            .ReturnsAsync(null as Assignment);
        _assignmentRepositoryMock.Setup(c => c.UnityOfWork.Commit()).ReturnsAsync(false);

        var assignmentInputModel = new AddAssignmentInputModel
        {
            Description = "Teste"
        };

        // Act
        var assignment = await _assignmentService.Create(assignmentInputModel);

        // Assert
        using (new AssertionScope())
        {
            assignment.Should().BeNull();
            Erros.Should().NotBeEmpty();
            Erros.Should().Contain("Não foi possível cadastrar a tarefa");
            _assignmentRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Once);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Once);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
        }
    }

    [Fact]
    public async Task Delete_Assignment()
    {
        // Arrange
        const int id = 1;
        SetupMockHttpContext();
        
        var assignment = new Assignment { Id = id };
        _assignmentRepositoryMock.Setup(c => c.UnityOfWork.Commit()).ReturnsAsync(true);
        _assignmentRepositoryMock.Setup(c => c.GetById(id, 1)).ReturnsAsync(assignment);

        // Act
        await _assignmentService.Delete(id);

        // Assert
        using (new AssertionScope())
        {
            Erros.Should().BeEmpty();
            _assignmentRepositoryMock.Verify(c => c.GetById(id, 1), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Once);
        }
    }

    [Fact]
    public async Task Delete_Assignment_ReturnHandleNotFoundResource()
    {
        // Arrange
        const int id = 1;
        SetupMockHttpContext();

        _assignmentRepositoryMock.Setup(c => c.GetById(id, 1)).ReturnsAsync(null as Assignment);

        // Act
        await _assignmentService.Delete(id);

        // Assert
        using (new AssertionScope())
        {
            NotFound.Should().BeTrue();
            NotificatorMock.Verify(c => c.HandleNotFoundResource(), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.GetById(id, 1), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Never);
        }
    }

    [Fact]
    public async Task Delete_Assignment_ReturnErrorUnitOfWorkCommit()
    {
        // Arrange
        const int id = 1;
        SetupMockHttpContext();

        var assignment = new Assignment { Id = id };
        _assignmentRepositoryMock.Setup(c => c.UnityOfWork.Commit()).ReturnsAsync(false);
        _assignmentRepositoryMock.Setup(c => c.GetById(id, 1)).ReturnsAsync(assignment);

        // Act
        await _assignmentService.Delete(id);

        // Assert
        using (new AssertionScope())
        {
            Erros.Should().NotBeEmpty();
            Erros.Should().Contain("Não foi possível remover a tarefa");
            _assignmentRepositoryMock.Verify(c => c.GetById(id, 1), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Once);
        }
    }

    [Fact]
    public async Task MarkConcluded_Assignment_ReturnAssignmentConcluded()
    {
        // Arrange
        const int id = 1;
        SetupMockHttpContext();

        var assignment = new Assignment { Id = id };
        _assignmentRepositoryMock.Setup(c => c.UnityOfWork.Commit()).ReturnsAsync(true);
        _assignmentRepositoryMock.Setup(c => c.GetById(id, 1)).ReturnsAsync(assignment);

        // Act
        await _assignmentService.MarkConcluded(id);

        // Assert
        using (new AssertionScope())
        {
            Erros.Should().BeEmpty();
            NotificatorMock.Verify(c => c.HandleNotFoundResource(), Times.Never);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Never);
            _assignmentRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.Update(assignment), Times.Once);
        }
    }

    [Fact]
    public async Task MarkConcluded_Assignment_ReturnNotFoundResource()
    {
        // Arrange
        const int id = 1;
        SetupMockHttpContext();

        _assignmentRepositoryMock.Setup(c => c.GetById(id, 1)).ReturnsAsync(null as Assignment);

        // Act
        await _assignmentService.MarkConcluded(id);

        // Assert
        using (new AssertionScope())
        {
            NotFound.Should().BeTrue();
            NotificatorMock.Verify(c => c.HandleNotFoundResource(), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.GetById(id, 1), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Never);
        }
    }

    [Fact]
    public async Task MarkConcluded_Assignment_ReturnErrorUnitOfWorkCommit()
    {
        // Arrange
        const int id = 1;
        SetupMockHttpContext();

        var assignment = new Assignment { Id = id };
        _assignmentRepositoryMock.Setup(c => c.GetById(id, 1)).ReturnsAsync(assignment);
        _assignmentRepositoryMock.Setup(c => c.UnityOfWork.Commit()).ReturnsAsync(false);

        // Act
        await _assignmentService.MarkConcluded(id);

        // Assert
        using (new AssertionScope())
        {
            Erros.Should().NotBeEmpty();
            Erros.Should().Contain("Não foi possível marcar a tarefa como concluída");
            _assignmentRepositoryMock.Verify(c => c.GetById(id, 1), Times.Once);
            NotificatorMock.Verify(c => c.HandleNotFoundResource(), Times.Never);
            _assignmentRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Once);
        }
    }
    
        [Fact]
    public async Task MarkDesconcluded_Assignment_ReturnAssignmentDesconcluded()
    {
        // Arrange
        const int id = 1;
        SetupMockHttpContext();

        var assignment = new Assignment { Id = id };
        _assignmentRepositoryMock.Setup(c => c.UnityOfWork.Commit()).ReturnsAsync(true);
        _assignmentRepositoryMock.Setup(c => c.GetById(id, 1)).ReturnsAsync(assignment);

        // Act
        await _assignmentService.MarkDesconcluded(id);

        // Assert
        using (new AssertionScope())
        {
            Erros.Should().BeEmpty();
            NotificatorMock.Verify(c => c.HandleNotFoundResource(), Times.Never);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Never);
            _assignmentRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.Update(assignment), Times.Once);
        }
    }

    [Fact]
    public async Task MarkDesconcluded_Assignment_ReturnNotFoundResource()
    {
        // Arrange
        const int id = 1;
        SetupMockHttpContext();

        _assignmentRepositoryMock.Setup(c => c.GetById(id, 1)).ReturnsAsync(null as Assignment);

        // Act
        await _assignmentService.MarkDesconcluded(id);

        // Assert
        using (new AssertionScope())
        {
            NotFound.Should().BeTrue();
            NotificatorMock.Verify(c => c.HandleNotFoundResource(), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.GetById(id, 1), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Never);
        }
    }

    [Fact]
    public async Task MarkDesconcluded_Assignment_ReturnErrorUnitOfWorkCommit()
    {
        // Arrange
        const int id = 1;
        SetupMockHttpContext();

        var assignment = new Assignment { Id = id };
        _assignmentRepositoryMock.Setup(c => c.GetById(id, 1)).ReturnsAsync(assignment);
        _assignmentRepositoryMock.Setup(c => c.UnityOfWork.Commit()).ReturnsAsync(false);

        // Act
        await _assignmentService.MarkDesconcluded(id);

        // Assert
        using (new AssertionScope())
        {
            Erros.Should().NotBeEmpty();
            Erros.Should().Contain("Não foi possível marcar a tarefa como não concluída");
            _assignmentRepositoryMock.Verify(c => c.GetById(id, 1), Times.Once);
            NotificatorMock.Verify(c => c.HandleNotFoundResource(), Times.Never);
            _assignmentRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Once);
        }
    }

    private void SetupMockHttpContext()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1")
        };

        HttpContextAccessorHelper.SetupHttpContextWithClaims(_httpContextAccessorMock, claims);
    }
}