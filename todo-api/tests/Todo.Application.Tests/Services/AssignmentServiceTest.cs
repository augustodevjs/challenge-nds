using Todo.Domain.Models;
using Todo.Domain.Filter;
using Todo.Infra.Data.Paged;
using System.Security.Claims;
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
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1")
        };

        var assignmentSearch = new AssignmentSearchInputModel
        {
            PerPage = 1,
            Page = 1
        };

        HttpContextAccessorHelper.SetupHttpContextWithClaims(_httpContextAccessorMock, claims);

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
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1")
        };

        HttpContextAccessorHelper.SetupHttpContextWithClaims(_httpContextAccessorMock, claims);
        _assignmentRepositoryMock.Setup(c => c.GetById(1, 1)).ReturnsAsync(new Assignment { Id = 1 });

        // Act
        var assignment = await _assignmentService.GetById(1);

        // Assert
        using (new AssertionScope())
        {
            assignment.Should().NotBeNull();
            assignment.Should().BeOfType<AssignmentViewModel>();
            NotFound.Should().BeFalse();
            NotificatorMock.Verify(c => c.HandleNotFoundResource(), Times.Never);
            _assignmentRepositoryMock.Verify(c => c.GetById(1, 1), Times.Once);
        }
    }

    [Fact]
    public async Task GetById_AssignmentNotExistent_ReturnNotFoundResource()
    {
        // Arrange
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1")
        };

        HttpContextAccessorHelper.SetupHttpContextWithClaims(_httpContextAccessorMock, claims);
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
}