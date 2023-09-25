// using Microsoft.AspNetCore.Http;
// using Moq;
// using Todo.Application.DTO.V1.ViewModel;
// using Todo.Application.Services;
// using Todo.Application.Tests.Fixtures;
// using Todo.Domain.Contracts.Repository;
// using Todo.Domain.Models;
//
// namespace Todo.Application.Tests.Services;
//
// public class AssignmentServiceTest : BaseServiceTest, IClassFixture<ServicesFixtures>
// {
//     private readonly ServicesFixtures _servicesFixtures;
//     private readonly AssignmentService _assignmentService;
//     private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
//     private readonly Mock<IAssignmentRepository> _assignmentRepositoryMock;
//     private readonly Mock<IAssignmentListRepository> _assignmentListRepositoryMock;
//
//     public AssignmentServiceTest(ServicesFixtures servicesFixtures)
//     {
//         _servicesFixtures = servicesFixtures;
//
//         _assignmentRepositoryMock = new Mock<IAssignmentRepository>();
//         _assignmentListRepositoryMock = new Mock<IAssignmentListRepository>();
//         _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
//
//         _assignmentService = new AssignmentService(
//             _servicesFixtures.Mapper,
//             NotificatorMock.Object,
//             _httpContextAccessorMock.Object,
//             _assignmentRepositoryMock.Object,
//             _assignmentListRepositoryMock.Object);
//     }
//
//     [Fact]
//     public async Task GetById_AssignmentExistent_ReturnAssignment()
//     {
//         // Arrange
//         _assignmentRepositoryMock.Setup(c => c.GetById(1, 1)).ReturnsAsync(new Assignment{ Id = 1});
//
//         // Act
//         var assignment = await _assignmentService.GetById(1);
//
//         // Assert
//         using (new AssertionScope())
//         {
//             assignment.Should().NotBeNull();
//             assignment.Should().BeOfType<AssignmentViewModel>();
//             NotFound.Should().BeFalse();
//             NotificatorMock.Verify(c => c.HandleNotFoundResource(), Times.Never);
//         }
//     }
//
//     [Fact]
//     public async Task GetById_NotExistentAssignment_ReturnHandleNotFoundResource()
//     {
//     }
// }