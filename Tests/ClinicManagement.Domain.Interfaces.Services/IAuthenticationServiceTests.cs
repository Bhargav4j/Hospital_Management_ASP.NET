using Xunit;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Interfaces.Services;
using System.Threading;
using System.Threading.Tasks;

namespace ClinicManagement.Domain.Interfaces.Services.Tests;

public class IAuthenticationServiceTests
{
    private class MockAuthenticationService : IAuthenticationService
    {
        public Task<(bool IsValid, UserType? UserType, int? UserId)> ValidateLoginAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            if (email == "valid@test.com" && password == "valid")
            {
                return Task.FromResult((true, (UserType?)UserType.Patient, (int?)1));
            }
            return Task.FromResult((false, (UserType?)null, (int?)null));
        }

        public Task<string> HashPasswordAsync(string password)
        {
            return Task.FromResult($"hashed_{password}");
        }

        public Task<bool> VerifyPasswordAsync(string password, string passwordHash)
        {
            return Task.FromResult(passwordHash == $"hashed_{password}");
        }
    }

    [Fact]
    public async Task ValidateLoginAsync_ShouldReturnTrue_WithValidCredentials()
    {
        var service = new MockAuthenticationService();

        var result = await service.ValidateLoginAsync("valid@test.com", "valid");

        Assert.True(result.IsValid);
        Assert.Equal(UserType.Patient, result.UserType);
        Assert.Equal(1, result.UserId);
    }

    [Fact]
    public async Task ValidateLoginAsync_ShouldReturnFalse_WithInvalidCredentials()
    {
        var service = new MockAuthenticationService();

        var result = await service.ValidateLoginAsync("invalid@test.com", "invalid");

        Assert.False(result.IsValid);
        Assert.Null(result.UserType);
        Assert.Null(result.UserId);
    }

    [Fact]
    public async Task HashPasswordAsync_ShouldReturnHashedPassword()
    {
        var service = new MockAuthenticationService();

        var result = await service.HashPasswordAsync("password123");

        Assert.NotNull(result);
        Assert.Equal("hashed_password123", result);
    }

    [Fact]
    public async Task VerifyPasswordAsync_ShouldReturnTrue_WhenPasswordMatches()
    {
        var service = new MockAuthenticationService();
        var password = "test123";
        var hash = await service.HashPasswordAsync(password);

        var result = await service.VerifyPasswordAsync(password, hash);

        Assert.True(result);
    }

    [Fact]
    public async Task VerifyPasswordAsync_ShouldReturnFalse_WhenPasswordDoesNotMatch()
    {
        var service = new MockAuthenticationService();
        var password = "test123";
        var hash = await service.HashPasswordAsync("different");

        var result = await service.VerifyPasswordAsync(password, hash);

        Assert.False(result);
    }
}
