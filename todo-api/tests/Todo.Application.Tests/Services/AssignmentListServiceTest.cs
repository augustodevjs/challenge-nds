using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Todo.Application.DTO.V1.InputModel;
using Todo.Application.DTO.V1.ViewModel;
using Todo.Application.Services;
using Todo.Application.Tests.Fixtures;
using Todo.Domain.Contracts.Repository;
using Todo.Domain.Filter;
using Todo.Domain.Models;
using Todo.Infra.Data.Paged;

namespace Todo.Application.Tests.Services;

public class AssignmentListServiceTest : BaseServiceTest, IClassFixture<ServicesFixtures>
{
    private readonly AssignmentListService _assignmentListService;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<IAssignmentRepository> _assignmentRepositoryMock;
    private readonly Mock<IAssignmentListRepository> _assignmentListRepositoryMock;

    public AssignmentListServiceTest(ServicesFixtures servicesFixtures)
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _assignmentListRepositoryMock = new Mock<IAssignmentListRepository>();
        _assignmentRepositoryMock = new Mock<IAssignmentRepository>();
        _assignmentListService = new AssignmentListService(
            servicesFixtures.Mapper,
            NotificatorMock.Object,
            _httpContextAccessorMock.Object,
            _assignmentRepositoryMock.Object,
            _assignmentListRepositoryMock.Object
        );
    }

    #region search

    [Fact]
    public async Task Search_ReturnPagedViewModelOfAssignmentListViewModel()
    {
        // Arrange
        SetupMocks();

        var assignmentListSearch = new AssignmentListSearchInputModel
        {
            PerPage = 1,
            Page = 1
        };

        // Act
        var assignmentListService = await _assignmentListService.Search(assignmentListSearch);

        // Assert
        using (new AssertionScope())
        {
            assignmentListService.Should().BeOfType<PagedViewModel<AssignmentListViewModel>>();
            _assignmentListRepositoryMock.Verify(c => c.Search
                    (It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()),
                Times.Once);
        }
    }

    [Fact]
    public async Task SearchAssignments_ReturnPagedViewModelOfAssignmentViewModel()
    {
        // Arrange
        SetupMocks();

        var assignmentSearch = new AssignmentSearchInputModel
        {
            PerPage = 1,
            Page = 1
        };

        // Act
        var assignmentListService = await _assignmentListService.SearchAssignments(1, assignmentSearch);

        // Assert
        using (new AssertionScope())
        {
            assignmentListService.Should().BeOfType<PagedViewModel<AssignmentViewModel>>();
            _assignmentRepositoryMock.Verify(c => c.Search(It.IsAny<int>(), It.IsAny<AssignmentFilter>(),
                assignmentSearch.PerPage,
                assignmentSearch.Page, It.IsAny<int>()), Times.Once);
        }
    }

    [Fact]
    public async Task SearchAssignments_ReturnHandleNotFoundResource()
    {
        // Arrange
        SetupMocks();

        var assignmentSearch = new AssignmentSearchInputModel
        {
            PerPage = 1,
            Page = 1
        };

        // Act
        var assignmentListService = await _assignmentListService.SearchAssignments(2, assignmentSearch);

        // Assert
        using (new AssertionScope())
        {
            assignmentListService.Should().BeNull();
            NotFound.Should().BeTrue();
            NotificatorMock.Verify(c => c.HandleNotFoundResource(), Times.Once);
            _assignmentListRepositoryMock.Verify(c => c.GetById(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.Search(It.IsAny<int>(), It.IsAny<AssignmentFilter>(),
                assignmentSearch.PerPage, assignmentSearch.Page, It.IsAny<int>()), Times.Never);
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
        var assignmentListService = await _assignmentListService.GetById(1);

        // Assert
        using (new AssertionScope())
        {
            NotFound.Should().BeFalse();
            assignmentListService.Should().NotBeNull();
            assignmentListService.Should().BeOfType<AssignmentListViewModel>();
            NotificatorMock.Verify(c => c.HandleNotFoundResource(), Times.Never);
            _assignmentListRepositoryMock.Verify(c => c.GetById(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }
    }

    [Fact]
    public async Task GetById_AssignmentNotExistent_ReturnHandleNotFoundResource()
    {
        // Arrange
        SetupMocks();

        // Act
        var assignmentListService = await _assignmentListService.GetById(2);

        // Assert
        using (new AssertionScope())
        {
            NotFound.Should().BeTrue();
            assignmentListService.Should().BeNull();
            NotificatorMock.Verify(c => c.HandleNotFoundResource(), Times.Once);
            _assignmentListRepositoryMock.Verify(c => c.GetById(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }
    }

    #endregion

    #region create

    [Fact]
    public async Task Create_AssignmentList_ReturnAssignmentListViewModel()
    {
        // Arrange
        SetupMocks(false);

        var assignmentListInputModel = new AddAssignmentListInputModel
        {
            Description = "Teste",
            Name = "Name"
        };

        // Act
        var assignmentListService = await _assignmentListService.Create(assignmentListInputModel);

        // Assert
        using (new AssertionScope())
        {
            assignmentListService.Should().NotBeNull();
            Erros.Should().BeEmpty();
            _assignmentListRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Once);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Never);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
        }
    }

    [Fact]
    public async Task Create_AssignmentList_HandleErrorValidation()
    {
        // Arrange
        SetupMocks(false);
        var assignmentListInputModel = new AddAssignmentListInputModel();

        // Act
        var assignmentListService = await _assignmentListService.Create(assignmentListInputModel);

        // Assert
        using (new AssertionScope())
        {
            Erros.Should().NotBeEmpty();
            assignmentListService.Should().BeNull();
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Never);
            _assignmentListRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Never);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Once);
        }
    }

    [Fact]
    public async Task Create_AssignmentList_HandleErrorWhenInfoAlreadyExist()
    {
        // Arrange
        SetupMocks();
        var assignmentListInputModel = new AddAssignmentListInputModel();

        // Act
        var assignmentListService = await _assignmentListService.Create(assignmentListInputModel);

        // Assert
        using (new AssertionScope())
        {
            Erros.Should().NotBeEmpty();
            assignmentListService.Should().BeNull();
            Erros.Should().Contain("Já existe uma lista de tarefa com esse nome.");
            NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Once);
        }
    }

    [Fact]
    public async Task Create_AssignmentList_HandleErrorUnityOfWorkCommit()
    {
        // Arrange
        SetupMocks(false, false);

        var assignmentListInputModel = new AddAssignmentListInputModel
        {
            Description = "Teste",
            Name = "Test Name"
        };

        // Act
        var assignment = await _assignmentListService.Create(assignmentListInputModel);

        // Assert
        using (new AssertionScope())
        {
            assignment.Should().BeNull();
            Erros.Should().NotBeEmpty();
            Erros.Should().Contain("Não foi possível criar a lista de tarefa");
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Once);
            _assignmentListRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Once);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
        }
    }

    #endregion

    #region update

    [Fact]
    public async Task Update_AssignmentList_ReturnAssignmentListViewModel()
    {
        // Arrange
        SetupMocks(false);

        var assignmentListInputModel = new UpdateAssignmentListInputModel
        {
            Id = 1,
            Description = "Teste",
            Name = "Name"
        };

        // Act
        var assignmentListService = await _assignmentListService.Update(1, assignmentListInputModel);

        // Assert
        using (new AssertionScope())
        {
            Erros.Should().BeEmpty();
            assignmentListService.Should().NotBeNull();
            assignmentListService.Should().BeOfType<AssignmentListViewModel>();
            _assignmentListRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Once);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Never);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
        }
    }

    [Fact]
    public async Task Update_AssignmentList_ReturnHandleErrorIdsNotEqual()
    {
        // Arrange
        SetupMocks(false);

        var assignmentListInputModel = new UpdateAssignmentListInputModel
        {
            Id = 1,
            Description = "Teste",
            Name = "Name"
        };

        // Act
        var assignmentListService = await _assignmentListService.Update(2, assignmentListInputModel);

        // Assert
        using (new AssertionScope())
        {
            Erros.Should().NotBeEmpty();
            assignmentListService.Should().BeNull();
            Erros.Should().Contain("Os ids não conferem");
            _assignmentListRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Never);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Once);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
        }
    }

    [Fact]
    public async Task Update_AssignmentList_ReturnHandleNotFoundResource()
    {
        // Arrange
        SetupMocks(false);

        var assignmentListInputModel = new UpdateAssignmentListInputModel
        {
            Id = 2,
            Description = "Teste",
            Name = "Name"
        };

        // Act
        var assignmentListService = await _assignmentListService.Update(2, assignmentListInputModel);

        // Assert
        using (new AssertionScope())
        {
            NotFound.Should().BeTrue();
            assignmentListService.Should().BeNull();
            NotificatorMock.Verify(c => c.HandleNotFoundResource(), Times.Once);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Never);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
            _assignmentListRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Never);
            _assignmentListRepositoryMock.Verify(c => c.GetById(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }
    }

    [Fact]
    public async Task Update_AssignmentList_HandleErrorValidation()
    {
        // Arrange
        SetupMocks(false);
        var assignmentListInputModel = new UpdateAssignmentListInputModel { Id = 1 };

        // Act
        var assignmentListService = await _assignmentListService.Update(1, assignmentListInputModel);

        // Assert
        using (new AssertionScope())
        {
            Erros.Should().NotBeEmpty();
            assignmentListService.Should().BeNull();
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Never);
            _assignmentListRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Never);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Once);
        }
    }

    [Fact]
    public async Task Update_AssignmentList_HandleErrorWhenInfoAlreadyExist()
    {
        // Arrange
        SetupMocks();
        var assignmentListInputModel = new UpdateAssignmentListInputModel
        {
            Id = 1,
            Description = "Teste",
            Name = "Name"
        };

        // Act
        var assignmentListService = await _assignmentListService.Update(1, assignmentListInputModel);

        // Assert
        using (new AssertionScope())
        {
            Erros.Should().NotBeEmpty();
            assignmentListService.Should().BeNull();
            Erros.Should().Contain("Já existe uma lista de tarefa com esse nome.");
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Once);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
        }
    }

    [Fact]
    public async Task Update_AssignmentList_HandleErrorUnityOfWorkCommit()
    {
        // Arrange
        SetupMocks(false, false);

        var assignmentListInputModel = new UpdateAssignmentListInputModel
        {
            Id = 1,
            Description = "Teste",
            Name = "Name"
        };

        // Act
        var assignment = await _assignmentListService.Update(1, assignmentListInputModel);

        // Assert
        using (new AssertionScope())
        {
            assignment.Should().BeNull();
            Erros.Should().NotBeEmpty();
            Erros.Should().Contain("Não foi possível atualizar a lista de tarefa");
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Once);
            _assignmentListRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Once);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
        }
    }

    #endregion

    #region delete

    [Fact]
    public async Task Delete_AssignmentList()
    {
        // Arrange
        SetupMocks();

        // Act
        await _assignmentListService.Delete(1);

        // Assert
        using (new AssertionScope())
        {
            Erros.Should().BeEmpty();
            _assignmentListRepositoryMock.Verify(c => c.GetById(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _assignmentListRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Once);
        }
    }

    [Fact]
    public async Task Delete_AssignmentList_ReturnHandleNotFoundResource()
    {
        // Arrange
        SetupMocks();

        // Act
        await _assignmentListService.Delete(2);

        // Assert
        using (new AssertionScope())
        {
            NotFound.Should().BeTrue();
            NotificatorMock.Verify(c => c.HandleNotFoundResource(), Times.Once);
            _assignmentListRepositoryMock.Verify(c => c.GetById(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _assignmentListRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Never);
        }
    }

    [Fact]
    public async Task Delete_AssignmentList_ReturnErrorUnitOfWorkCommit()
    {
        // Arrange
        SetupMocks(true, false);

        // Act
        await _assignmentListService.Delete(1);

        // Assert
        using (new AssertionScope())
        {
            Erros.Should().NotBeEmpty();
            Erros.Should().Contain("Não foi possível remover a lista de tarefa.");
            _assignmentListRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Once);
            _assignmentListRepositoryMock.Verify(c => c.GetById(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }
    }

    [Fact]
    public async Task Delete_AssignmentList_ReturnHandleErrorListAlreadyNotConcluded()
    {
        // Arrange
        SetupMocks(true, false);

        // Act
        await _assignmentListService.Delete(3);

        // Assert
        using (new AssertionScope())
        {
            Erros.Should().NotBeEmpty();
            Erros.Should().Contain("Não é possível excluir lista com tarefas não concluídas.");
            NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Once);
            NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
            _assignmentListRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Never);
            _assignmentListRepositoryMock.Verify(c => c.GetById(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }
    }

    #endregion

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

    private void SetupMocks(bool firstDefault = true, bool commit = true)
    {
        SetupMockContextAcessor();

        var assignment = new Assignment();
        var assignmentList = new AssignmentList { Id = 1 };

        assignment.SetUnconcluded();

        _assignmentListRepositoryMock
            .Setup(c => c.GetById(It.Is<int>(x => x == 1), 1))
            .ReturnsAsync(assignmentList);

        _assignmentListRepositoryMock
            .Setup(c => c.GetById(It.Is<int>(x => x == 2), 1))
            .ReturnsAsync(null as AssignmentList);

        _assignmentListRepositoryMock
            .Setup(c => c.GetById(It.Is<int>(x => x == 3), 1))
            .ReturnsAsync(new AssignmentList
            {
                Id = 1, Assignments =
                {
                    assignment
                }
            });

        _assignmentListRepositoryMock
            .Setup(c => c.Search(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(),
                It.IsAny<int>())).ReturnsAsync(new PagedResult<AssignmentList>()
            {
                Items = new List<AssignmentList>()
            });

        _assignmentRepositoryMock
            .Setup(c => c.Search(It.IsAny<int>(), It.IsAny<AssignmentFilter>(), It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<int>()))
            .ReturnsAsync(new PagedResult<Assignment>
            {
                Items = new List<Assignment>()
            });

        _assignmentListRepositoryMock.Setup(c => c.FirstOrDefault(It.IsAny<Expression<Func<AssignmentList, bool>>>()))
            .ReturnsAsync(firstDefault ? assignmentList : null);

        _assignmentListRepositoryMock.Setup(c => c.UnityOfWork.Commit()).ReturnsAsync(commit);
    }
}