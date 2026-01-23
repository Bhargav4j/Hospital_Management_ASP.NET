using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClinicManagement.Domain.Interfaces.Services;
using ClinicManagement.Web.Pages;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClinicManagement.Web.Pages.Tests;

/// <summary>
/// Unit tests for IndexModel
/// </summary>
public class IndexModelTests
{
    private readonly Mock<IAuthenticationService> _mockAuthService;
    private readonly Mock<ILogger<IndexModel>> _mockLogger;

    public IndexModelTests()
    {
        _mockAuthService = new Mock<IAuthenticationService>();
        _mockLogger = new Mock<ILogger<IndexModel>>();
    }

    private IndexModel CreateIndexModel()
    {
        var model = new IndexModel(_mockAuthService.Object, _mockLogger.Object);
        model.PageContext = new PageContext
        {
            HttpContext = new DefaultHttpContext()
        };
        return model;
    }

    [Fact]
    public void IndexModel_Constructor_WithNullAuthService_ShouldThrowArgumentNullException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new IndexModel(null!, _mockLogger.Object));
    }

    [Fact]
    public void IndexModel_Constructor_WithNullLogger_ShouldThrowArgumentNullException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new IndexModel(_mockAuthService.Object, null!));
    }

    [Fact]
    public void OnGet_ShouldClearSession()
    {
        // Arrange
        var model = CreateIndexModel();
        var session = new Mock<ISession>();
        model.HttpContext.Session = session.Object;

        // Act
        model.OnGet();

        // Assert
        session.Verify(s => s.Clear(), Times.Once);
    }

    [Fact]
    public async Task OnPostLoginAsync_WithValidPatientCredentials_ShouldRedirectToPatientHome()
    {
        // Arrange
        var model = CreateIndexModel();
        var sessionData = new Dictionary<string, byte[]>();

        var mockSession = new Mock<ISession>();
        mockSession.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
            .Callback<string, byte[]>((key, value) => sessionData[key] = value);

        model.HttpContext.Session = mockSession.Object;

        _mockAuthService.Setup(s => s.ValidateLoginAsync("patient@test.com", "password", It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, 1, "Patient", "Login successful"));

        // Act
        var result = await model.OnPostLoginAsync("patient@test.com", "password");

        // Assert
        var redirectResult = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Patient/PatientHome", redirectResult.PageName);
    }

    [Fact]
    public async Task OnPostLoginAsync_WithValidDoctorCredentials_ShouldRedirectToDoctorHome()
    {
        // Arrange
        var model = CreateIndexModel();
        var sessionData = new Dictionary<string, byte[]>();

        var mockSession = new Mock<ISession>();
        mockSession.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
            .Callback<string, byte[]>((key, value) => sessionData[key] = value);

        model.HttpContext.Session = mockSession.Object;

        _mockAuthService.Setup(s => s.ValidateLoginAsync("doctor@test.com", "password", It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, 2, "Doctor", "Login successful"));

        // Act
        var result = await model.OnPostLoginAsync("doctor@test.com", "password");

        // Assert
        var redirectResult = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Doctor/DoctorHome", redirectResult.PageName);
    }

    [Fact]
    public async Task OnPostLoginAsync_WithInvalidCredentials_ShouldReturnPageWithErrorMessage()
    {
        // Arrange
        var model = CreateIndexModel();

        _mockAuthService.Setup(s => s.ValidateLoginAsync("invalid@test.com", "wrongpassword", It.IsAny<CancellationToken>()))
            .ReturnsAsync((false, 0, string.Empty, "Invalid credentials"));

        // Act
        var result = await model.OnPostLoginAsync("invalid@test.com", "wrongpassword");

        // Assert
        Assert.IsType<PageResult>(result);
        Assert.Equal("Invalid credentials", model.Message);
        Assert.False(model.IsSuccess);
    }

    [Fact]
    public async Task OnPostSignUpAsync_WithValidData_ShouldRedirectToPatientHome()
    {
        // Arrange
        var model = CreateIndexModel();
        var sessionData = new Dictionary<string, byte[]>();

        var mockSession = new Mock<ISession>();
        mockSession.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
            .Callback<string, byte[]>((key, value) => sessionData[key] = value);

        model.HttpContext.Session = mockSession.Object;

        _mockAuthService.Setup(s => s.RegisterPatientAsync(
            It.IsAny<string>(),
            It.IsAny<DateTime>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, 10, "Registration successful"));

        // Act
        var result = await model.OnPostSignUpAsync(
            "John Doe",
            "1990-01-01",
            "john@test.com",
            "password123",
            "1234567890",
            "Male",
            "123 Main St");

        // Assert
        var redirectResult = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Patient/PatientHome", redirectResult.PageName);
    }

    [Fact]
    public async Task OnPostSignUpAsync_WithInvalidBirthDate_ShouldReturnPageWithErrorMessage()
    {
        // Arrange
        var model = CreateIndexModel();

        // Act
        var result = await model.OnPostSignUpAsync(
            "John Doe",
            "invalid-date",
            "john@test.com",
            "password123",
            "1234567890",
            "Male",
            "123 Main St");

        // Assert
        Assert.IsType<PageResult>(result);
        Assert.Equal("Invalid birth date format.", model.Message);
        Assert.False(model.IsSuccess);
    }

    [Fact]
    public async Task OnPostSignUpAsync_WithExistingEmail_ShouldReturnPageWithErrorMessage()
    {
        // Arrange
        var model = CreateIndexModel();

        _mockAuthService.Setup(s => s.RegisterPatientAsync(
            It.IsAny<string>(),
            It.IsAny<DateTime>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync((false, 0, "Email already registered"));

        // Act
        var result = await model.OnPostSignUpAsync(
            "Jane Doe",
            "1990-01-01",
            "existing@test.com",
            "password123",
            "1234567890",
            "Female",
            "456 Oak St");

        // Assert
        Assert.IsType<PageResult>(result);
        Assert.Equal("Email already registered", model.Message);
        Assert.False(model.IsSuccess);
    }

    [Fact]
    public void IndexModel_Properties_ShouldBeInitializable()
    {
        // Arrange
        var model = CreateIndexModel();

        // Act
        model.Message = "Test message";
        model.IsSuccess = true;

        // Assert
        Assert.Equal("Test message", model.Message);
        Assert.True(model.IsSuccess);
    }
}
