using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using HospitalManagement.Domain.Interfaces.Services;
using HospitalManagement.Web.Pages;

namespace HospitalManagement.Web.Pages.Tests;

public class LoginModelTests
{
    private readonly Mock<IAuthenticationService> _mockAuthService;
    private readonly Mock<ILogger<LoginModel>> _mockLogger;
    private readonly Mock<HttpContext> _mockHttpContext;
    private readonly Mock<ISession> _mockSession;

    public LoginModelTests()
    {
        _mockAuthService = new Mock<IAuthenticationService>();
        _mockLogger = new Mock<ILogger<LoginModel>>();
        _mockHttpContext = new Mock<HttpContext>();
        _mockSession = new Mock<ISession>();

        _mockHttpContext.Setup(x => x.Session).Returns(_mockSession.Object);
    }

    [Fact]
    public void LoginModel_Constructor_WithValidParameters_CreatesInstance()
    {
        // Arrange & Act
        var model = new LoginModel(_mockAuthService.Object, _mockLogger.Object);

        // Assert
        Assert.NotNull(model);
    }

    [Fact]
    public void LoginModel_Constructor_WithNullAuthService_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new LoginModel(null!, _mockLogger.Object));
    }

    [Fact]
    public void LoginModel_Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new LoginModel(_mockAuthService.Object, null!));
    }

    [Fact]
    public void LoginModel_Properties_InitializeWithDefaultValues()
    {
        // Arrange & Act
        var model = new LoginModel(_mockAuthService.Object, _mockLogger.Object);

        // Assert
        Assert.Equal(string.Empty, model.Email);
        Assert.Equal(string.Empty, model.Password);
        Assert.Null(model.ErrorMessage);
    }

    [Fact]
    public void OnGet_ExecutesSuccessfully()
    {
        // Arrange
        var model = new LoginModel(_mockAuthService.Object, _mockLogger.Object);

        // Act
        model.OnGet();

        // Assert - method completes without exception
        Assert.True(true);
    }

    [Fact]
    public void LoginModel_SetEmail_StoresValue()
    {
        // Arrange
        var model = new LoginModel(_mockAuthService.Object, _mockLogger.Object);
        var expectedEmail = "test@example.com";

        // Act
        model.Email = expectedEmail;

        // Assert
        Assert.Equal(expectedEmail, model.Email);
    }

    [Fact]
    public void LoginModel_SetPassword_StoresValue()
    {
        // Arrange
        var model = new LoginModel(_mockAuthService.Object, _mockLogger.Object);
        var expectedPassword = "SecurePassword123";

        // Act
        model.Password = expectedPassword;

        // Assert
        Assert.Equal(expectedPassword, model.Password);
    }

    [Fact]
    public void LoginModel_SetErrorMessage_StoresValue()
    {
        // Arrange
        var model = new LoginModel(_mockAuthService.Object, _mockLogger.Object);
        var expectedMessage = "Error occurred";

        // Act
        model.ErrorMessage = expectedMessage;

        // Assert
        Assert.Equal(expectedMessage, model.ErrorMessage);
    }

    [Fact]
    public async Task OnPostAsync_WithInvalidModelState_ReturnsPage()
    {
        // Arrange
        var model = new LoginModel(_mockAuthService.Object, _mockLogger.Object);
        model.ModelState.AddModelError("Email", "Email is required");

        // Act
        var result = await model.OnPostAsync();

        // Assert
        Assert.IsType<PageResult>(result);
    }

    [Fact]
    public async Task OnPostAsync_WithValidPatientCredentials_RedirectsToPatientDashboard()
    {
        // Arrange
        var model = new LoginModel(_mockAuthService.Object, _mockLogger.Object)
        {
            PageContext = new PageContext { HttpContext = _mockHttpContext.Object }
        };

        model.Email = "patient@test.com";
        model.Password = "password123";

        _mockAuthService
            .Setup(x => x.ValidateLoginAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, 1, "Patient"));

        // Act
        var result = await model.OnPostAsync();

        // Assert
        var redirectResult = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Patients/Dashboard", redirectResult.PageName);
    }

    [Fact]
    public async Task OnPostAsync_WithValidDoctorCredentials_RedirectsToDoctorDashboard()
    {
        // Arrange
        var model = new LoginModel(_mockAuthService.Object, _mockLogger.Object)
        {
            PageContext = new PageContext { HttpContext = _mockHttpContext.Object }
        };

        model.Email = "doctor@test.com";
        model.Password = "password123";

        _mockAuthService
            .Setup(x => x.ValidateLoginAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, 2, "Doctor"));

        // Act
        var result = await model.OnPostAsync();

        // Assert
        var redirectResult = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Doctors/Dashboard", redirectResult.PageName);
    }

    [Fact]
    public async Task OnPostAsync_WithInvalidCredentials_ReturnsPageWithError()
    {
        // Arrange
        var model = new LoginModel(_mockAuthService.Object, _mockLogger.Object)
        {
            PageContext = new PageContext { HttpContext = _mockHttpContext.Object }
        };

        model.Email = "invalid@test.com";
        model.Password = "wrongpassword";

        _mockAuthService
            .Setup(x => x.ValidateLoginAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((false, 0, string.Empty));

        // Act
        var result = await model.OnPostAsync();

        // Assert
        Assert.IsType<PageResult>(result);
        Assert.Equal("Invalid email or password", model.ErrorMessage);
    }

    [Fact]
    public async Task OnPostAsync_WithException_ReturnsPageWithError()
    {
        // Arrange
        var model = new LoginModel(_mockAuthService.Object, _mockLogger.Object)
        {
            PageContext = new PageContext { HttpContext = _mockHttpContext.Object }
        };

        model.Email = "test@test.com";
        model.Password = "password";

        _mockAuthService
            .Setup(x => x.ValidateLoginAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await model.OnPostAsync();

        // Assert
        Assert.IsType<PageResult>(result);
        Assert.Equal("An error occurred during login. Please try again.", model.ErrorMessage);
    }

    [Fact]
    public async Task OnPostAsync_WithSuccessfulPatientLogin_SetsSessionValues()
    {
        // Arrange
        var model = new LoginModel(_mockAuthService.Object, _mockLogger.Object)
        {
            PageContext = new PageContext { HttpContext = _mockHttpContext.Object }
        };

        model.Email = "patient@test.com";
        model.Password = "password123";

        _mockAuthService
            .Setup(x => x.ValidateLoginAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, 10, "Patient"));

        // Act
        await model.OnPostAsync();

        // Assert
        _mockSession.Verify(x => x.Set("UserId", It.IsAny<byte[]>()), Times.Once);
        _mockSession.Verify(x => x.Set("UserType", It.IsAny<byte[]>()), Times.Once);
    }

    [Fact]
    public async Task OnPostAsync_WithSuccessfulDoctorLogin_SetsSessionValues()
    {
        // Arrange
        var model = new LoginModel(_mockAuthService.Object, _mockLogger.Object)
        {
            PageContext = new PageContext { HttpContext = _mockHttpContext.Object }
        };

        model.Email = "doctor@test.com";
        model.Password = "password123";

        _mockAuthService
            .Setup(x => x.ValidateLoginAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, 20, "Doctor"));

        // Act
        await model.OnPostAsync();

        // Assert
        _mockSession.Verify(x => x.Set("UserId", It.IsAny<byte[]>()), Times.Once);
        _mockSession.Verify(x => x.Set("UserType", It.IsAny<byte[]>()), Times.Once);
    }

    [Fact]
    public async Task OnPostAsync_WithSuccessfulLogin_LogsInformation()
    {
        // Arrange
        var model = new LoginModel(_mockAuthService.Object, _mockLogger.Object)
        {
            PageContext = new PageContext { HttpContext = _mockHttpContext.Object }
        };

        model.Email = "user@test.com";
        model.Password = "password";

        _mockAuthService
            .Setup(x => x.ValidateLoginAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, 5, "Patient"));

        // Act
        await model.OnPostAsync();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("logged in")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task OnPostAsync_WithException_LogsError()
    {
        // Arrange
        var model = new LoginModel(_mockAuthService.Object, _mockLogger.Object)
        {
            PageContext = new PageContext { HttpContext = _mockHttpContext.Object }
        };

        model.Email = "error@test.com";
        model.Password = "password";

        _mockAuthService
            .Setup(x => x.ValidateLoginAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        await model.OnPostAsync();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Theory]
    [InlineData("patient@test.com", "pass123", "Patient", "/Patients/Dashboard")]
    [InlineData("doctor@test.com", "pass456", "Doctor", "/Doctors/Dashboard")]
    public async Task OnPostAsync_WithDifferentUserTypes_RedirectsCorrectly(string email, string password, string userType, string expectedPage)
    {
        // Arrange
        var model = new LoginModel(_mockAuthService.Object, _mockLogger.Object)
        {
            PageContext = new PageContext { HttpContext = _mockHttpContext.Object }
        };

        model.Email = email;
        model.Password = password;

        _mockAuthService
            .Setup(x => x.ValidateLoginAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, 1, userType));

        // Act
        var result = await model.OnPostAsync();

        // Assert
        var redirectResult = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal(expectedPage, redirectResult.PageName);
    }
}
