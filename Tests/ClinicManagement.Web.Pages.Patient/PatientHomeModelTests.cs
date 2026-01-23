using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Web.Pages.Patient;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClinicManagement.Web.Pages.Patient.Tests;

/// <summary>
/// Unit tests for PatientHomeModel
/// </summary>
public class PatientHomeModelTests
{
    private readonly Mock<IPatientRepository> _mockPatientRepository;
    private readonly Mock<ILogger<PatientHomeModel>> _mockLogger;

    public PatientHomeModelTests()
    {
        _mockPatientRepository = new Mock<IPatientRepository>();
        _mockLogger = new Mock<ILogger<PatientHomeModel>>();
    }

    private PatientHomeModel CreatePatientHomeModel()
    {
        var model = new PatientHomeModel(_mockPatientRepository.Object, _mockLogger.Object);
        model.PageContext = new PageContext
        {
            HttpContext = new DefaultHttpContext()
        };
        return model;
    }

    [Fact]
    public void PatientHomeModel_Constructor_WithNullPatientRepository_ShouldThrowArgumentNullException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new PatientHomeModel(null!, _mockLogger.Object));
    }

    [Fact]
    public void PatientHomeModel_Constructor_WithNullLogger_ShouldThrowArgumentNullException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new PatientHomeModel(_mockPatientRepository.Object, null!));
    }

    [Fact]
    public async Task OnGetAsync_WithValidSession_ShouldLoadPatientName()
    {
        // Arrange
        var model = CreatePatientHomeModel();
        var mockSession = new Mock<ISession>();

        mockSession.Setup(s => s.TryGetValue("UserId", out It.Ref<byte[]>.IsAny))
            .Returns((string key, out byte[] value) =>
            {
                value = BitConverter.GetBytes(1);
                return true;
            });

        model.HttpContext.Session = mockSession.Object;

        var patient = new Domain.Entities.Patient
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@test.com",
            IsActive = true
        };

        _mockPatientRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        // Act
        var result = await model.OnGetAsync();

        // Assert
        Assert.IsType<PageResult>(result);
        Assert.Equal("John Doe", model.PatientName);
    }

    [Fact]
    public async Task OnGetAsync_WithoutSession_ShouldRedirectToIndex()
    {
        // Arrange
        var model = CreatePatientHomeModel();
        var mockSession = new Mock<ISession>();

        mockSession.Setup(s => s.TryGetValue("UserId", out It.Ref<byte[]>.IsAny))
            .Returns(false);

        model.HttpContext.Session = mockSession.Object;

        // Act
        var result = await model.OnGetAsync();

        // Assert
        var redirectResult = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Index", redirectResult.PageName);
    }

    [Fact]
    public async Task OnGetAsync_WithNullPatient_ShouldReturnPageWithDefaultName()
    {
        // Arrange
        var model = CreatePatientHomeModel();
        var mockSession = new Mock<ISession>();

        mockSession.Setup(s => s.TryGetValue("UserId", out It.Ref<byte[]>.IsAny))
            .Returns((string key, out byte[] value) =>
            {
                value = BitConverter.GetBytes(1);
                return true;
            });

        model.HttpContext.Session = mockSession.Object;

        _mockPatientRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Patient?)null);

        // Act
        var result = await model.OnGetAsync();

        // Assert
        Assert.IsType<PageResult>(result);
        Assert.Equal("Patient", model.PatientName);
    }

    [Fact]
    public void PatientHomeModel_PatientName_DefaultValue_ShouldBePatient()
    {
        // Arrange & Act
        var model = CreatePatientHomeModel();

        // Assert
        Assert.Equal("Patient", model.PatientName);
    }

    [Fact]
    public void PatientHomeModel_PatientName_ShouldBeSettable()
    {
        // Arrange
        var model = CreatePatientHomeModel();

        // Act
        model.PatientName = "Test Patient";

        // Assert
        Assert.Equal("Test Patient", model.PatientName);
    }
}
