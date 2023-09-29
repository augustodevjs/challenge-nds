using Todo.Domain.Models;
using Todo.Application.Services;
using Microsoft.AspNetCore.Identity;
using Todo.Application.Tests.Fixtures;
using Todo.Domain.Contracts.Repository;
using Microsoft.Extensions.Configuration;
using Todo.Application.DTO.V1.InputModel;

namespace Todo.Application.Tests.Services;

public class AuthServiceTest : BaseServiceTest, IClassFixture<ServicesFixtures>
{
    private readonly AuthService _authService;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IConfiguration> _configurationMock = new();
    private readonly Mock<IPasswordHasher<User>> _passwordHasherUserMock = new();

    public AuthServiceTest(ServicesFixtures servicesFixtures)
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _authService = new AuthService(
            servicesFixtures.Mapper,
            NotificatorMock.Object,
            _configurationMock.Object,
            _userRepositoryMock.Object,
            _passwordHasherUserMock.Object
        );
    }

    #region login

    [Fact]
    public async Task Login_ValidUser_ReturnsToken()
    {
        // Arrange
        SetupMocks();
        var inputModel = new LoginInputModel
        {
            Email = "novo@gmail.com",
            Password = "SENHACORRETA"
        };

        // Act
        var result = await _authService.Login(inputModel);

        // Assert
        result.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Login_InvalidUser_ReturnsHandleErrors()
    {
        // Arrange
        SetupMocks();
        var inputModel = new LoginInputModel();

        // Act
        var result = await _authService.Login(inputModel);

        // Assert
        result.Should().BeNull();
        Erros.Should().NotBeEmpty();
        Erros.Should().HaveCountGreaterOrEqualTo(2);
        NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Once);
    }
    
    [Fact]
    public async Task Login_NotExistentEmailOrPasswordVerificationResultFailed_ReturnHandleError()
    {
        // Arrange
        SetupMocks();
        var inputModel = new LoginInputModel
        {
            Email = "teste@gmail.com",
            Password = "SENHAINCORRETA"
        };

        // Act
        var result = await _authService.Login(inputModel);

        // Assert
        result.Should().BeNull();
        Erros.Should().NotBeEmpty();
        Erros.Should().Contain("Usuário ou senha estão incorretos.");
        NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
        NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Once);
    }

    #endregion

    #region register

    [Fact]
    public async Task Register_User_ReturnUserViewModel()
    {
        // Arrange
        SetupMocks();

        var inputModel = new RegisterInputModel
        {
            Name = "teste",
            Email = "teste@gmail.com",
            Password = "123",
            ConfirmPassword = "123"
        };

        // Act
        var user = await _authService.Register(inputModel);

        // Assert
        user.Should().NotBeNull();
        Erros.Should().BeEmpty();
        _userRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Once);
        NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Never);
        NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
    }

    [Fact]
    public async Task Register_User_ReturnErrorValidation()
    {
        // Arrange
        var inputModel = new RegisterInputModel();

        // Act
        var user = await _authService.Register(inputModel);

        // Assert
        user.Should().BeNull();
        Erros.Should().NotBeEmpty();
        Erros.Should().HaveCountGreaterThan(2);
        NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Once);
    }

    [Fact]
    public async Task Register_User_ReturnHandleErrorEmail()
    {
        // Arrange
        SetupMocks();

        var inputModel = new RegisterInputModel
        {
            Name = "Teste",
            Email = "error@gmail.com",
            Password = "123",
            ConfirmPassword = "123"
        };

        // Act
        var user = await _authService.Register(inputModel);

        // Assert
        user.Should().BeNull();
        Erros.Should().NotBeEmpty();
        Erros.Should().Contain("Já existe um usuário cadastrado com o email informado.");
        _userRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Never);
        NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Once);
        NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
    }

    [Fact]
    public async Task Register_User_ReturnHandleErrorCommit()
    {
        // Arrange
        SetupMocks(false);

        var inputModel = new RegisterInputModel
        {
            Name = "Teste",
            Email = "teste@gmail.com",
            Password = "123",
            ConfirmPassword = "123"
        };

        // Act
        var user = await _authService.Register(inputModel);

        // Assert
        user.Should().BeNull();
        Erros.Should().NotBeEmpty();
        Erros.Should().Contain("Não foi possível cadastrar o usuário");
        _userRepositoryMock.Verify(c => c.UnityOfWork.Commit(), Times.Once);
        NotificatorMock.Verify(c => c.Handle(It.IsAny<string>()), Times.Once);
        NotificatorMock.Verify(c => c.Handle(It.IsAny<List<ValidationFailure>>()), Times.Never);
    }

    #endregion

    #region mock

    private void SetupMocks(bool commit = true)
    {
        _userRepositoryMock
            .Setup(c => c.GetByEmail(It.Is<string>(x => x != "teste@gmail.com")))
            .ReturnsAsync(new User { Id = 1, Name = "teste", Password = "123"});

        _userRepositoryMock
            .Setup(c => c.GetByEmail(It.Is<string>(x => x == "teste@gmail.com")))
            .ReturnsAsync(null as User);

        _passwordHasherUserMock
            .Setup(c => c.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(),
                It.Is<string>(x => x == "SENHACORRETA"))).Returns(PasswordVerificationResult.Success);

        _passwordHasherUserMock
            .Setup(c => c.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(),
                It.Is<string>(x => x == "SENHAINCORRETA"))).Returns(PasswordVerificationResult.Failed);

        _configurationMock
            .Setup(c => c["AppSettings:Secret"])
            .Returns("MEUSEGREDOSUPERSECRETO");

        _configurationMock
            .Setup(c => c["AppSettings:ExpirationHours"])
            .Returns("1");

        _configurationMock
            .Setup(c => c["AppSettings:Issuer"])
            .Returns("TODO");

        _configurationMock
            .Setup(c => c["AppSettings:ValidOn"])
            .Returns("https://localhost");

        _userRepositoryMock.Setup(c => c.UnityOfWork.Commit()).ReturnsAsync(commit);
    }

    #endregion
}