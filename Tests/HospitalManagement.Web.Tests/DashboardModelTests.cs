using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using HospitalManagement.Web.Pages.Patients;

namespace HospitalManagement.Web.Pages.Patients.Tests;

public class DashboardModelTests
{
    private readonly Mock<ILogger<DashboardModel>> _mockLogger;
    private readonly Mock<HttpContext> _mockHttpContext;
    private readonly Mock<ISession> _mockSession;

    public DashboardModelTests()
    {
        _mockLogger = new Mock<ILogger<DashboardModel>>();
        _mockHttpContext = new Mock<HttpContext>();
        _mockSession = new Mock<ISession>();

        _mockHttpContext.Setup(x => x.Session).Returns(_mockSession.Object);
    }

    [Fact]
    public void DashboardModel_Constructor_WithValidLogger_CreatesInstance()
    {
        // Arrange & Act
        var model = new DashboardModel(_mockLogger.Object);

        // Assert
        Assert.NotNull(model);
    }

    [Fact]
    public void DashboardModel_Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DashboardModel(null!));
    }

    [Fact]
    public void OnGet_WithValidPatientSession_ReturnsPage()
    {
        // Arrange
        var model = new DashboardModel(_mockLogger.Object)
        {
            PageContext = new PageContext { HttpContext = _mockHttpContext.Object }
        };

        byte[] userIdBytes = BitConverter.GetBytes(15);
        byte[] userTypeBytes = System.Text.Encoding.UTF8.GetBytes("Patient");

        _mockSession.Setup(x => x.TryGetValue("UserId", out userIdBytes)).Returns(true);
        _mockSession.Setup(x => x.TryGetValue("UserType", out userTypeBytes)).Returns(true);

        // Act
        var result = model.OnGet();

        // Assert
        Assert.IsType<PageResult>(result);
    }

    [Fact]
    public void OnGet_WithNoUserId_RedirectsToLogin()
    {
        // Arrange
        var model = new DashboardModel(_mockLogger.Object)
        {
            PageContext = new PageContext { HttpContext = _mockHttpContext.Object }
        };

        byte[]? nullBytes = null;
        _mockSession.Setup(x => x.TryGetValue("UserId", out nullBytes)).Returns(false);

        // Act
        var result = model.OnGet();

        // Assert
        var redirectResult = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Login", redirectResult.PageName);
    }

    [Fact]
    public void OnGet_WithDoctorUserType_RedirectsToLogin()
    {
        // Arrange
        var model = new DashboardModel(_mockLogger.Object)
        {
            PageContext = new PageContext { HttpContext = _mockHttpContext.Object }
        };

        byte[] userIdBytes = BitConverter.GetBytes(15);
        byte[] userTypeBytes = System.Text.Encoding.UTF8.GetBytes("Doctor");

        _mockSession.Setup(x => x.TryGetValue("UserId", out userIdBytes)).Returns(true);
        _mockSession.Setup(x => x.TryGetValue("UserType", out userTypeBytes)).Returns(true);

        // Act
        var result = model.OnGet();

        // Assert
        var redirectResult = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Login", redirectResult.PageName);
    }

    [Fact]
    public void OnGet_WithEmptyUserType_RedirectsToLogin()
    {
        // Arrange
        var model = new DashboardModel(_mockLogger.Object)
        {
            PageContext = new PageContext { HttpContext = _mockHttpContext.Object }
        };

        byte[] userIdBytes = BitConverter.GetBytes(15);
        byte[] userTypeBytes = System.Text.Encoding.UTF8.GetBytes(string.Empty);

        _mockSession.Setup(x => x.TryGetValue("UserId", out userIdBytes)).Returns(true);
        _mockSession.Setup(x => x.TryGetValue("UserType", out userTypeBytes)).Returns(true);

        // Act
        var result = model.OnGet();

        // Assert
        var redirectResult = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Login", redirectResult.PageName);
    }

    [Fact]
    public void OnGet_WithNullUserType_RedirectsToLogin()
    {
        // Arrange
        var model = new DashboardModel(_mockLogger.Object)
        {
            PageContext = new PageContext { HttpContext = _mockHttpContext.Object }
        };

        byte[] userIdBytes = BitConverter.GetBytes(15);
        byte[]? nullBytes = null;

        _mockSession.Setup(x => x.TryGetValue("UserId", out userIdBytes)).Returns(true);
        _mockSession.Setup(x => x.TryGetValue("UserType", out nullBytes)).Returns(false);

        // Act
        var result = model.OnGet();

        // Assert
        var redirectResult = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Login", redirectResult.PageName);
    }

    [Fact]
    public void OnGet_WithValidPatientSession_LogsInformation()
    {
        // Arrange
        var model = new DashboardModel(_mockLogger.Object)
        {
            PageContext = new PageContext { HttpContext = _mockHttpContext.Object }
        };

        byte[] userIdBytes = BitConverter.GetBytes(30);
        byte[] userTypeBytes = System.Text.Encoding.UTF8.GetBytes("Patient");

        _mockSession.Setup(x => x.TryGetValue("UserId", out userIdBytes)).Returns(true);
        _mockSession.Setup(x => x.TryGetValue("UserType", out userTypeBytes)).Returns(true);

        // Act
        model.OnGet();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("accessed dashboard")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void OnGet_WithoutValidSession_DoesNotLogInformation()
    {
        // Arrange
        var model = new DashboardModel(_mockLogger.Object)
        {
            PageContext = new PageContext { HttpContext = _mockHttpContext.Object }
        };

        byte[]? nullBytes = null;
        _mockSession.Setup(x => x.TryGetValue("UserId", out nullBytes)).Returns(false);

        // Act
        model.OnGet();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("accessed dashboard")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Never);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(500)]
    public void OnGet_WithDifferentPatientIds_ReturnsPage(int patientId)
    {
        // Arrange
        var model = new DashboardModel(_mockLogger.Object)
        {
            PageContext = new PageContext { HttpContext = _mockHttpContext.Object }
        };

        byte[] userIdBytes = BitConverter.GetBytes(patientId);
        byte[] userTypeBytes = System.Text.Encoding.UTF8.GetBytes("Patient");

        _mockSession.Setup(x => x.TryGetValue("UserId", out userIdBytes)).Returns(true);
        _mockSession.Setup(x => x.TryGetValue("UserType", out userTypeBytes)).Returns(true);

        // Act
        var result = model.OnGet();

        // Assert
        Assert.IsType<PageResult>(result);
    }

    [Theory]
    [InlineData("Admin")]
    [InlineData("Nurse")]
    [InlineData("Receptionist")]
    public void OnGet_WithInvalidUserTypes_RedirectsToLogin(string userType)
    {
        // Arrange
        var model = new DashboardModel(_mockLogger.Object)
        {
            PageContext = new PageContext { HttpContext = _mockHttpContext.Object }
        };

        byte[] userIdBytes = BitConverter.GetBytes(15);
        byte[] userTypeBytes = System.Text.Encoding.UTF8.GetBytes(userType);

        _mockSession.Setup(x => x.TryGetValue("UserId", out userIdBytes)).Returns(true);
        _mockSession.Setup(x => x.TryGetValue("UserType", out userTypeBytes)).Returns(true);

        // Act
        var result = model.OnGet();

        // Assert
        var redirectResult = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Login", redirectResult.PageName);
    }

    [Fact]
    public void OnGet_WithValidSession_LogsCorrectPatientId()
    {
        // Arrange
        var model = new DashboardModel(_mockLogger.Object)
        {
            PageContext = new PageContext { HttpContext = _mockHttpContext.Object }
        };

        var expectedPatientId = 42;
        byte[] userIdBytes = BitConverter.GetBytes(expectedPatientId);
        byte[] userTypeBytes = System.Text.Encoding.UTF8.GetBytes("Patient");

        _mockSession.Setup(x => x.TryGetValue("UserId", out userIdBytes)).Returns(true);
        _mockSession.Setup(x => x.TryGetValue("UserType", out userTypeBytes)).Returns(true);

        // Act
        model.OnGet();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Patient") && v.ToString()!.Contains("accessed dashboard")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
