using Xunit;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Interfaces.Services;
using ClinicManagement.Web.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;

namespace ClinicManagement.Web.Pages.Tests;

public class LoginModelTests
{
    private readonly Mock<IAuthenticationService> _authServiceMock;
    private readonly Mock<ILogger<LoginModel>> _loggerMock;

    public LoginModelTests()
    {
        _authServiceMock = new Mock<IAuthenticationService>();
        _loggerMock = new Mock<ILogger<LoginModel>>();
    }

    [Fact]
    public void LoginModel_Constructor_ShouldInitializeProperties()
    {
        var model = new LoginModel(_authServiceMock.Object, _loggerMock.Object);

        Assert.NotNull(model);
        Assert.Equal(string.Empty, model.Email);
        Assert.Equal(string.Empty, model.Password);
    }

    [Fact]
    public void OnGet_ShouldNotThrow()
    {
        var model = new LoginModel(_authServiceMock.Object, _loggerMock.Object);

        var exception = Record.Exception(() => model.OnGet());

        Assert.Null(exception);
    }

    [Fact]
    public async Task OnPostAsync_ShouldReturnPage_WhenModelStateInvalid()
    {
        var model = new LoginModel(_authServiceMock.Object, _loggerMock.Object);
        model.ModelState.AddModelError("Email", "Required");

        var result = await model.OnPostAsync();

        Assert.IsType<PageResult>(result);
    }

    [Fact]
    public async Task OnPostAsync_ShouldRedirectToPatientIndex_WhenPatientLoginSuccessful()
    {
        _authServiceMock
            .Setup(x => x.ValidateLoginAsync(It.IsAny<string>(), It.IsAny<string>(), default))
            .ReturnsAsync((true, UserType.Patient, 1));

        var httpContext = new DefaultHttpContext();
        httpContext.Session = new Mock<ISession>().Object;

        var model = new LoginModel(_authServiceMock.Object, _loggerMock.Object)
        {
            Email = "patient@test.com",
            Password = "password",
            PageContext = new PageContext
            {
                HttpContext = httpContext
            }
        };

        var result = await model.OnPostAsync();

        var redirectResult = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Patients/Index", redirectResult.PageName);
    }

    [Fact]
    public async Task OnPostAsync_ShouldReturnPage_WithErrorMessage_WhenLoginFails()
    {
        _authServiceMock
            .Setup(x => x.ValidateLoginAsync(It.IsAny<string>(), It.IsAny<string>(), default))
            .ReturnsAsync((false, null, null));

        var model = new LoginModel(_authServiceMock.Object, _loggerMock.Object)
        {
            Email = "invalid@test.com",
            Password = "wrongpassword",
            PageContext = new PageContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        var result = await model.OnPostAsync();

        Assert.IsType<PageResult>(result);
        Assert.Equal("Invalid email or password", model.ErrorMessage);
    }

    [Fact]
    public async Task OnPostAsync_ShouldHandleException_AndReturnErrorMessage()
    {
        _authServiceMock
            .Setup(x => x.ValidateLoginAsync(It.IsAny<string>(), It.IsAny<string>(), default))
            .ThrowsAsync(new Exception("Test exception"));

        var model = new LoginModel(_authServiceMock.Object, _loggerMock.Object)
        {
            Email = "test@test.com",
            Password = "password",
            PageContext = new PageContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        var result = await model.OnPostAsync();

        Assert.IsType<PageResult>(result);
        Assert.Contains("error occurred", model.ErrorMessage);
    }

    [Fact]
    public async Task OnPostAsync_ShouldRedirectToDoctorIndex_WhenDoctorLoginSuccessful()
    {
        _authServiceMock
            .Setup(x => x.ValidateLoginAsync(It.IsAny<string>(), It.IsAny<string>(), default))
            .ReturnsAsync((true, UserType.Doctor, 2));

        var httpContext = new DefaultHttpContext();
        httpContext.Session = new Mock<ISession>().Object;

        var model = new LoginModel(_authServiceMock.Object, _loggerMock.Object)
        {
            Email = "doctor@test.com",
            Password = "password",
            PageContext = new PageContext
            {
                HttpContext = httpContext
            }
        };

        var result = await model.OnPostAsync();

        var redirectResult = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Doctors/Index", redirectResult.PageName);
    }

    [Fact]
    public async Task OnPostAsync_ShouldRedirectToAdminIndex_WhenAdminLoginSuccessful()
    {
        _authServiceMock
            .Setup(x => x.ValidateLoginAsync(It.IsAny<string>(), It.IsAny<string>(), default))
            .ReturnsAsync((true, UserType.Admin, 3));

        var httpContext = new DefaultHttpContext();
        httpContext.Session = new Mock<ISession>().Object;

        var model = new LoginModel(_authServiceMock.Object, _loggerMock.Object)
        {
            Email = "admin@test.com",
            Password = "password",
            PageContext = new PageContext
            {
                HttpContext = httpContext
            }
        };

        var result = await model.OnPostAsync();

        var redirectResult = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Admin/Index", redirectResult.PageName);
    }
}
