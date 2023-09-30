using Todo.Domain.Models;
using Todo.Domain.Filter;
using Todo.Infra.Data.Paged;
using System.Security.Claims;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Todo.Application.Services;
using Todo.Application.Tests.Fixtures;
using Todo.Domain.Contracts.Repository;
using Todo.Application.DTO.V1.ViewModel;
using Todo.Application.DTO.V1.InputModel;

namespace Todo.Application.Tests.Services;

public class AssignmentServiceTest : BaseServiceTest, IClassFixture<ServicesFixtures>
{
    private readonly AssignmentService _assignmentService;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<IAssignmentRepository> _assignmentRepositoryMock;
    private readonly Mock<IAssignmentListRepository> _assignmentListRepositoryMock;

    public AssignmentServiceTest(ServicesFixtures servicesFixtures)
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _assignmentRepositoryMock = new Mock<IAssignmentRepository>();
        _assignmentListRepositoryMock = new Mock<IAssignmentListRepository>();
        _assignmentService = new AssignmentService(
            servicesFixtures.Mapper,
            NotificatorMock.Object,
            _httpContextAccessorMock.Object,
            _assignmentRepositoryMock.Object,
            _assignmentListRepositoryMock.Object
        );
    }

    #region search

    [Fact]
    public async Task Search_ReturnPagedViewModelOfAssignmentViewModel()
    {
        // Arrange
        SetupMocks();

        var assignmentSearch = new AssignmentSearchInputModel
        {
            PerPage = 1,
            Page = 1
        };

        // Act
        var search = await _assignmentService.Search(assignmentSearch);

        // Assert

        using (new AssertionScope())
        {
            search.Should().BeOfType<PagedViewModel<AssignmentViewModel>>();
            _assignmentRepositoryMock.Verify(c => c.Search(It.IsAny<int>(), It.IsAny<AssignmentFilter>(),
                assignmentSearch.PerPage,
                assignmentSearch.Page, null), Times.Once);
        }
    }

    #endregion

    #region getById

    [Fact]
    public async Task GetById_AssignmentExistent_ReturnAssignment()
    {
        // Arrange
        SetupMocks();

        // Act
        var assignmentService = await _assignmentService.GetById(1);

        // Assert
        using (new AssertionScope())
        {
            NotFound.Should().BeFalse();
            assignmentService.Should().NotBeNull();
            assignmentService.Should().BeOfType<AssignmentViewModel>();
            NotificatorMock.Verify(c => c.HandleNotFoundResource(), Times.Never);
            _assignmentRepositoryMock.Verify(c => c.GetById(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }
    }

    [Fact]
    public async Task GetById_AssignmentNotExistent_ReturnNotFoundResource()
    {
        // Arrange
        SetupMocks();

        // Act
        var assignment = await _assignmentService.GetById(2);

        // Assert
        using (new AssertionScope())
        {
            assignment.Should().BeNull();
            NotFound.Should().BeTrue();
            NotificatorMock.Verify(c => c.HandleNotFoundResource(), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.GetById(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }
    }

    #endregion

    #region create

    [Fact]
    public async Task Create_Assignment_ReturnAssignmentViewModel()
    {
        // Arrange
        SetupMocks(false);

        var assignmentInputModel = new AddAssignmentInputModel
        {
            Description = "Teste",
            AssignmentListId = 1,
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
        SetupMocks(false);
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
    public async Task Create_Assignment_HandleErrorWhenAssignmentListAlreadyExist()
    {
        // Arrange
        SetupMocks(false, true, false);
        var assignmentInputModel = new AddAssignmentInputModel
        {
            Description = "teste",
            AssignmentListId = 1
        };
    
        // Act
        var assignmentService = await _assignmentService.Create(assignmentInputModel);
    
        // Assert
        using (new AssertionScope())
        {
            Erros.Should().NotBeEmpty();
            assignmentService.Should().BeNull();
            Erros.Should().Contain("Não existe essa lista de tarefa.");
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Once);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
            _assignmentListRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Never);

        }
    }
    
    [Fact]
    public async Task Create_Assignment_HandleErrorWhenAssignmentHaveAlreadyRegisteredInfo()
    {
        // Arrange
        SetupMocks();
        var assignmentInputModel = new AddAssignmentInputModel
        {
            Description = "teste",
            AssignmentListId = 1
        };
    
        // Act
        var assignmentService = await _assignmentService.Create(assignmentInputModel);
    
        // Assert
        using (new AssertionScope())
        {
            Erros.Should().NotBeEmpty();
            assignmentService.Should().BeNull();
            Erros.Should().Contain("Já existe uma tarefa cadastrada com essas informações.");
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Once);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
            _assignmentListRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Never);

        }
    }
    
    [Fact]
    public async Task Create_Assignment_HandleErrorUnityOfWorkCommit()
    {
        // Arrange
        SetupMocks(false, false);
    
        var assignmentInputModel = new AddAssignmentInputModel
        {
            Description = "Teste",
            AssignmentListId = 1
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

    #endregion

    #region update
    
    [Fact]
    public async Task Update_Assignment_ReturnUpdatedAssignmentViewModel()
    {
        // Arrange
        SetupMocks(false, true);
        var assignmentInputModel = new UpdateAssignmentInputModel
            { Id = 1, Description = "Teste", AssignmentListId = 1 };
    
        // Act
        var assignment = await _assignmentService.Update(1, assignmentInputModel);
    
        // Assert
        using (new AssertionScope())
        {
            Erros.Should().BeEmpty();
            NotFound.Should().BeFalse();
            assignment.Should().NotBeNull();
            assignment.Should().BeOfType<AssignmentViewModel>();
            _assignmentRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.Update(It.IsAny<Assignment>()), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.GetById(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Never);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
        }
    }
    
    [Fact]
    public async Task Update_InvalidId_ReturnHandleError()
    {
        // Arrange
        SetupMocks();
    
        // Act
        var assignment = await _assignmentService.Update(1, new UpdateAssignmentInputModel { Id = 2 });
    
        // Assert
        using (new AssertionScope())
        {
            assignment.Should().BeNull();
            Erros.Should().Contain("Os ids não conferem");
            _assignmentRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Never);
            _assignmentRepositoryMock.Verify(c => c.GetById(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _assignmentRepositoryMock.Verify(c => c.Update(It.IsAny<Assignment>()), Times.Never);
        }
    }
    
    [Fact]
    public async Task Update_Assignment_ReturnNotFoundResource()
    {
        // Arrange
        SetupMocks();
    
        // Act
        var assignment = await _assignmentService.Update(2, new UpdateAssignmentInputModel { Id = 2 });
    
        // Assert
        using (new AssertionScope())
        {
            Erros.Should().BeEmpty();
            NotFound.Should().BeTrue();
            assignment.Should().BeNull();
            _assignmentRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Never);
            _assignmentRepositoryMock.Verify(c => c.GetById(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.Update(It.IsAny<Assignment>()), Times.Never);
        }
    }
    
    [Fact]
    public async Task Update_Assignment_ReturnHandleErrorValidation()
    {
        // Arrange
        SetupMocks(false);
        var assignmentInputModel = new UpdateAssignmentInputModel { Id = 1, Description = "", AssignmentListId = 0 };
    
        // Act
        var assignment = await _assignmentService.Update(1, assignmentInputModel);
    
        // Assert
        using (new AssertionScope())
        {
            assignment.Should().BeNull();
            Erros.Should().NotBeEmpty();
            Erros.Should().HaveCountGreaterThan(2);
            NotFound.Should().BeFalse();
            _assignmentRepositoryMock.Verify(c => c.GetById(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.Update(It.IsAny<Assignment>()), Times.Never);
            _assignmentRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Never);
    
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Never);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Once);
        }
    }
    
    [Fact]
    public async Task Update_Assignment_HandleErrorWhenAssignmentListAlreadyExist()
    {
        // Arrange
        SetupMocks(false, true, false);
        var assignmentInputModel = new UpdateAssignmentInputModel { Id = 1, Description = "Teste", AssignmentListId = 1 };
    
        // Act
        var assignmentService = await _assignmentService.Update(1, assignmentInputModel);
    
        // Assert
        using (new AssertionScope())
        {
            Erros.Should().NotBeEmpty();
            assignmentService.Should().BeNull();
            Erros.Should().Contain("Não existe essa lista de tarefa.");
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Once);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
            _assignmentListRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Never);

        }
    }
    
    [Fact]
    public async Task Update_Assignment_HandleErrorWhenAssignmentHaveAlreadyRegisteredInfo()
    {
        // Arrange
        SetupMocks();
        var assignmentInputModel = new UpdateAssignmentInputModel { Id = 1, Description = "Teste", AssignmentListId = 1 };
    
        // Act
        var assignmentService = await _assignmentService.Update(1 ,assignmentInputModel);
    
        // Assert
        using (new AssertionScope())
        {
            Erros.Should().NotBeEmpty();
            assignmentService.Should().BeNull();
            Erros.Should().Contain("Já existe uma tarefa cadastrada com essas informações.");
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Once);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
            _assignmentListRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Never);

        }
    }
    
    [Fact]
    public async Task Update_Assignment_ReturnHandleErrorUpdateAssignment()
    {
        // Arrange
        SetupMocks(false, false);
        var assignmentInputModel = new UpdateAssignmentInputModel
            { Id = 1, Description = "Teste", AssignmentListId = 1 };
    
        // Act
        var assignment = await _assignmentService.Update(1, assignmentInputModel);
    
        // Assert
        using (new AssertionScope())
        {
            assignment.Should().BeNull();
            Erros.Should().NotBeEmpty();
            Erros.Should().Contain("Não foi possível atualizar a tarefa");
            NotFound.Should().BeFalse();
            _assignmentRepositoryMock.Verify(c => c.GetById(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.Update(It.IsAny<Assignment>()), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Once);
    
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Once);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
        }
    }
    
    #endregion

    #region delete

    [Fact]
    public async Task Delete_Assignment()
    {
        // Arrange
        SetupMocks();

        // Act
        await _assignmentService.Delete(1);

        // Assert
        using (new AssertionScope())
        {
            Erros.Should().BeEmpty();
            _assignmentRepositoryMock.Verify(c => c.GetById(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Once);
        }
    }

    [Fact]
    public async Task Delete_Assignment_ReturnHandleNotFoundResource()
    {
        // Arrange
        SetupMocks();

        // Act
        await _assignmentService.Delete(2);

        // Assert
        using (new AssertionScope())
        {
            NotFound.Should().BeTrue();
            NotificatorMock.Verify(c => c.HandleNotFoundResource(), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.GetById(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Never);
        }
    }

    [Fact]
    public async Task Delete_Assignment_ReturnErrorUnitOfWorkCommit()
    {
        // Arrange
        SetupMocks(true, false);

        // Act
        await _assignmentService.Delete(1);

        // Assert
        using (new AssertionScope())
        {
            Erros.Should().NotBeEmpty();
            Erros.Should().Contain("Não foi possível remover a tarefa");
            _assignmentRepositoryMock.Verify(c => c.GetById(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Once);
        }
    }

    #endregion

    #region markConcluded

    [Fact]
    public async Task MarkConcluded_Assignment_ReturnAssignmentConcluded()
    {
        // Arrange
        SetupMocks();

        // Act
        await _assignmentService.MarkConcluded(1);

        // Assert
        using (new AssertionScope())
        {
            Erros.Should().BeEmpty();
            NotificatorMock.Verify(c => c.HandleNotFoundResource(), Times.Never);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Never);
            _assignmentRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.Update(It.IsAny<Assignment>()), Times.Once);
        }
    }

    [Fact]
    public async Task MarkConcluded_Assignment_ReturnNotFoundResource()
    {
        // Arrange
        SetupMocks();

        // Act
        await _assignmentService.MarkConcluded(2);

        // Assert
        using (new AssertionScope())
        {
            NotFound.Should().BeTrue();
            NotificatorMock.Verify(c => c.HandleNotFoundResource(), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.GetById(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Never);
        }
    }

    [Fact]
    public async Task MarkConcluded_Assignment_ReturnErrorUnitOfWorkCommit()
    {
        // Arrange
        SetupMocks(true, false);

        // Act
        await _assignmentService.MarkConcluded(1);

        // Assert
        using (new AssertionScope())
        {
            Erros.Should().NotBeEmpty();
            Erros.Should().Contain("Não foi possível marcar a tarefa como concluída");
            _assignmentRepositoryMock.Verify(c => c.GetById(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            NotificatorMock.Verify(c => c.HandleNotFoundResource(), Times.Never);
            _assignmentRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Once);
        }
    }

    #endregion

    #region markDesconcluded

    [Fact]
    public async Task MarkDesconcluded_Assignment_ReturnAssignmentDesconcluded()
    {
        // Arrange
        SetupMocks();

        // Act
        await _assignmentService.MarkDesconcluded(1);

        // Assert
        using (new AssertionScope())
        {
            Erros.Should().BeEmpty();
            NotificatorMock.Verify(c => c.HandleNotFoundResource(), Times.Never);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Never);
            _assignmentRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.Update(It.IsAny<Assignment>()), Times.Once);
        }
    }

    [Fact]
    public async Task MarkDesconcluded_Assignment_ReturnNotFoundResource()
    {
        // Arrange
        SetupMocks();

        // Act
        await _assignmentService.MarkDesconcluded(2);

        // Assert
        using (new AssertionScope())
        {
            NotFound.Should().BeTrue();
            NotificatorMock.Verify(c => c.HandleNotFoundResource(), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.GetById(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Never);
        }
    }

    [Fact]
    public async Task MarkDesconcluded_Assignment_ReturnErrorUnitOfWorkCommit()
    {
        // Arrange
        SetupMocks(true, false);

        // Act
        await _assignmentService.MarkDesconcluded(1);

        // Assert
        using (new AssertionScope())
        {
            Erros.Should().NotBeEmpty();
            Erros.Should().Contain("Não foi possível marcar a tarefa como não concluída");
            _assignmentRepositoryMock.Verify(c => c.GetById(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            NotificatorMock.Verify(c => c.HandleNotFoundResource(), Times.Never);
            _assignmentRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Once);
        }
    }

    #endregion

    #region mock

    private void SetupMockContextAcessor()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1")
        };

        var context = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(claims))
        };

        _httpContextAccessorMock.Setup(c => c.HttpContext).Returns(context);
    }

    private void SetupMocks(bool firstDefaultAssignment = true, bool commit = true,
        bool firstDefaultAssignmentList = true)
    {
        SetupMockContextAcessor();

        var assignment = new Assignment { Id = 1 };
        var assignmentList = new AssignmentList { Id = 1 };

        _assignmentRepositoryMock
            .Setup(c => c.GetById(It.Is<int>(x => x == 1), 1))
            .ReturnsAsync(assignment);

        _assignmentRepositoryMock
            .Setup(c => c.GetById(It.Is<int>(x => x != 1), 1))
            .ReturnsAsync(null as Assignment);

        _assignmentRepositoryMock
            .Setup(c => c.Search(1, It.IsAny<AssignmentFilter>(), It.Is<int>(x => x == 1), It.Is<int>(x => x == 1),
                null)).ReturnsAsync(new PagedResult<Assignment>
            {
                Items = new List<Assignment>()
            });

        _assignmentRepositoryMock.Setup(c => c.FirstOrDefault(It.IsAny<Expression<Func<Assignment, bool>>>()))
            .ReturnsAsync(firstDefaultAssignment ? assignment : null);

        _assignmentListRepositoryMock.Setup(c => c.FirstOrDefault(It.IsAny<Expression<Func<AssignmentList, bool>>>()))
            .ReturnsAsync(firstDefaultAssignmentList ? assignmentList : null);

        _assignmentRepositoryMock.Setup(c => c.UnityOfWork.Commit()).ReturnsAsync(commit);
    }

    #endregion
}