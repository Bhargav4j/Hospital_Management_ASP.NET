using Xunit;
using Moq;
using ClinicManagement.Web.Controllers;
using ClinicManagement.Application.Interfaces;
using ClinicManagement.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ClinicManagement.Tests.Unit.Web.Controllers;

public class AccountControllerTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly Mock<ILogger<AccountController>> _mockLogger;
    private readonly AccountController _controller;
    private readonly Mock<HttpContext> _mockHttpContext;
    private readonly Mock<ISession> _mockSession;

    public AccountControllerTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _mockLogger = new Mock<ILogger<AccountController>>();
        _controller = new AccountController(_mockAuthService.Object, _mockLogger.Object);

        _mockHttpContext = new Mock<HttpContext>();
        _mockSession = new Mock<ISession>();
        _mockHttpContext.Setup(c => c.Session).Returns(_mockSession.Object);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = _mockHttpContext.Object
        };

        _controller.TempData = new TempDataDictionary(_mockHttpContext.Object, Mock.Of<ITempDataProvider>());
    }

    [Fact]
    public void Login_Get_ShouldReturnView()
    {
        // Act
        var result = _controller.Login();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Login_Post_WithValidCredentials_ShouldRedirectToAdminIndex()
    {
        // Arrange
        var loginDto = new LoginDto { Email = "admin@example.com", Password = "password" };
        var loginResponse = new LoginResponseDto
        {
            Success = true,
            UserId = 1,
            UserType = 1,
            Token = "token123"
        };

        _mockAuthService.Setup(s => s.LoginAsync(It.IsAny<LoginDto>())).ReturnsAsync(loginResponse);

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Admin", redirectResult.ControllerName);
    }

    [Fact]
    public async Task Login_Post_WithValidCredentials_ShouldRedirectToDoctorIndex()
    {
        // Arrange
        var loginDto = new LoginDto { Email = "doctor@example.com", Password = "password" };
        var loginResponse = new LoginResponseDto
        {
            Success = true,
            UserId = 2,
            UserType = 2,
            Token = "token456"
        };

        _mockAuthService.Setup(s => s.LoginAsync(It.IsAny<LoginDto>())).ReturnsAsync(loginResponse);

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Doctor", redirectResult.ControllerName);
    }

    [Fact]
    public async Task Login_Post_WithValidCredentials_ShouldRedirectToPatientIndex()
    {
        // Arrange
        var loginDto = new LoginDto { Email = "patient@example.com", Password = "password" };
        var loginResponse = new LoginResponseDto
        {
            Success = true,
            UserId = 3,
            UserType = 3,
            Token = "token789"
        };

        _mockAuthService.Setup(s => s.LoginAsync(It.IsAny<LoginDto>())).ReturnsAsync(loginResponse);

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Patient", redirectResult.ControllerName);
    }

    [Fact]
    public async Task Login_Post_WithInvalidUserType_ShouldRedirectToLogin()
    {
        // Arrange
        var loginDto = new LoginDto { Email = "user@example.com", Password = "password" };
        var loginResponse = new LoginResponseDto
        {
            Success = true,
            UserId = 4,
            UserType = 99,
            Token = "token000"
        };

        _mockAuthService.Setup(s => s.LoginAsync(It.IsAny<LoginDto>())).ReturnsAsync(loginResponse);

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Login", redirectResult.ActionName);
    }

    [Fact]
    public async Task Login_Post_WithInvalidCredentials_ShouldReturnViewWithError()
    {
        // Arrange
        var loginDto = new LoginDto { Email = "wrong@example.com", Password = "wrongpassword" };
        var loginResponse = new LoginResponseDto { Success = false };

        _mockAuthService.Setup(s => s.LoginAsync(It.IsAny<LoginDto>())).ReturnsAsync(loginResponse);

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(string.Empty));
    }

    [Fact]
    public async Task Login_Post_WithInvalidModelState_ShouldReturnView()
    {
        // Arrange
        var loginDto = new LoginDto { Email = "", Password = "" };
        _controller.ModelState.AddModelError("Email", "Email is required");

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(loginDto, viewResult.Model);
    }

    [Fact]
    public async Task Login_Post_WhenExceptionThrown_ShouldReturnViewWithError()
    {
        // Arrange
        var loginDto = new LoginDto { Email = "test@example.com", Password = "password" };
        _mockAuthService.Setup(s => s.LoginAsync(It.IsAny<LoginDto>())).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(string.Empty));
    }

    [Fact]
    public void Register_Get_ShouldReturnView()
    {
        // Act
        var result = _controller.Register();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Register_Post_WithValidData_ShouldRedirectToLogin()
    {
        // Arrange
        var createUserDto = new CreateUserDto
        {
            Name = "New User",
            Email = "newuser@example.com",
            Password = "password123"
        };

        _mockAuthService.Setup(s => s.RegisterUserAsync(It.IsAny<CreateUserDto>())).ReturnsAsync(1);

        // Act
        var result = await _controller.Register(createUserDto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Login", redirectResult.ActionName);
        Assert.NotNull(_controller.TempData["SuccessMessage"]);
    }

    [Fact]
    public async Task Register_Post_WithExistingEmail_ShouldReturnViewWithError()
    {
        // Arrange
        var createUserDto = new CreateUserDto
        {
            Name = "Existing User",
            Email = "existing@example.com",
            Password = "password123"
        };

        _mockAuthService.Setup(s => s.RegisterUserAsync(It.IsAny<CreateUserDto>())).ReturnsAsync(-1);

        // Act
        var result = await _controller.Register(createUserDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(string.Empty));
    }

    [Fact]
    public async Task Register_Post_WithInvalidModelState_ShouldReturnView()
    {
        // Arrange
        var createUserDto = new CreateUserDto { Name = "", Email = "" };
        _controller.ModelState.AddModelError("Name", "Name is required");

        // Act
        var result = await _controller.Register(createUserDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(createUserDto, viewResult.Model);
    }

    [Fact]
    public async Task Register_Post_WhenExceptionThrown_ShouldReturnViewWithError()
    {
        // Arrange
        var createUserDto = new CreateUserDto
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "password"
        };

        _mockAuthService.Setup(s => s.RegisterUserAsync(It.IsAny<CreateUserDto>())).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.Register(createUserDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(string.Empty));
    }

    [Fact]
    public void Logout_ShouldClearSessionAndRedirectToLogin()
    {
        // Act
        var result = _controller.Logout();

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Login", redirectResult.ActionName);
        _mockSession.Verify(s => s.Clear(), Times.Once);
    }

    [Fact]
    public async Task Login_Post_WithSuccessfulLogin_ShouldSetSessionValues()
    {
        // Arrange
        var loginDto = new LoginDto { Email = "user@example.com", Password = "password" };
        var loginResponse = new LoginResponseDto
        {
            Success = true,
            UserId = 10,
            UserType = 2,
            Token = "validToken"
        };

        byte[] userIdBytes = null!;
        byte[] userTypeBytes = null!;
        byte[] tokenBytes = null!;

        _mockSession.Setup(s => s.Set("UserId", It.IsAny<byte[]>()))
            .Callback<string, byte[]>((key, value) => userIdBytes = value);
        _mockSession.Setup(s => s.Set("UserType", It.IsAny<byte[]>()))
            .Callback<string, byte[]>((key, value) => userTypeBytes = value);
        _mockSession.Setup(s => s.Set("Token", It.IsAny<byte[]>()))
            .Callback<string, byte[]>((key, value) => tokenBytes = value);

        _mockAuthService.Setup(s => s.LoginAsync(It.IsAny<LoginDto>())).ReturnsAsync(loginResponse);

        // Act
        await _controller.Login(loginDto);

        // Assert
        _mockSession.Verify(s => s.Set("UserId", It.IsAny<byte[]>()), Times.Once);
        _mockSession.Verify(s => s.Set("UserType", It.IsAny<byte[]>()), Times.Once);
        _mockSession.Verify(s => s.Set("Token", It.IsAny<byte[]>()), Times.Once);
    }

    [Theory]
    [InlineData(1, "Admin")]
    [InlineData(2, "Doctor")]
    [InlineData(3, "Patient")]
    public async Task Login_Post_WithDifferentUserTypes_ShouldRedirectToCorrectController(int userType, string expectedController)
    {
        // Arrange
        var loginDto = new LoginDto { Email = "user@example.com", Password = "password" };
        var loginResponse = new LoginResponseDto
        {
            Success = true,
            UserId = 1,
            UserType = userType,
            Token = "token"
        };

        _mockAuthService.Setup(s => s.LoginAsync(It.IsAny<LoginDto>())).ReturnsAsync(loginResponse);

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(expectedController, redirectResult.ControllerName);
    }

    [Fact]
    public async Task Register_Post_WithSuccessfulRegistration_ShouldSetSuccessMessage()
    {
        // Arrange
        var createUserDto = new CreateUserDto
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "password"
        };

        _mockAuthService.Setup(s => s.RegisterUserAsync(It.IsAny<CreateUserDto>())).ReturnsAsync(100);

        // Act
        await _controller.Register(createUserDto);

        // Assert
        Assert.NotNull(_controller.TempData["SuccessMessage"]);
        Assert.Equal("Registration successful. Please login.", _controller.TempData["SuccessMessage"]);
    }
}
