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
        var search = await _assignmentListService.Search(assignmentListSearch);

        // Assert
        using (new AssertionScope())
        {
            search.Should().BeOfType<PagedViewModel<AssignmentListViewModel>>();
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
        var search = await _assignmentListService.SearchAssignments(1, assignmentSearch);

        // Assert
        using (new AssertionScope())
        {
            search.Should().BeOfType<PagedViewModel<AssignmentViewModel>>();
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
        var search = await _assignmentListService.SearchAssignments(2, assignmentSearch);

        // Assert
        using (new AssertionScope())
        {
            search.Should().BeNull();
            NotFound.Should().BeTrue();
            NotificatorMock.Verify(c => c.HandleNotFoundResource(), Times.Once);
            _assignmentListRepositoryMock.Verify(c => c.GetById(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _assignmentRepositoryMock.Verify(c => c.Search(It.IsAny<int>(), It.IsAny<AssignmentFilter>(),
                assignmentSearch.PerPage,
                assignmentSearch.Page, It.IsAny<int>()), Times.Never);
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

        var assignemntList = new AssignmentList { Id = 1 };

        _assignmentListRepositoryMock
            .Setup(c => c.GetById(It.Is<int>(x => x == 1), 1))
            .ReturnsAsync(assignemntList);

        _assignmentListRepositoryMock
            .Setup(c => c.GetById(It.Is<int>(x => x != 1), 1))
            .ReturnsAsync(null as AssignmentList);

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

        // _assignmentRepositoryMock.Setup(c => c.FirstOrDefault(It.IsAny<Expression<Func<Assignment, bool>>>()))
        //     .ReturnsAsync(firstDefault ? assignemnt : null);
        //
        // _assignmentRepositoryMock.Setup(c => c.UnityOfWork.Commit()).ReturnsAsync(commit);
    }
}