using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Enums;
using HospitalManagement.Infrastructure.Data;
using HospitalManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.HospitalManagement.Infrastructure.Repositories;

public class UserRepositoryTests
{
    private readonly Mock<ILogger<UserRepository>> _mockLogger;
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

    public UserRepositoryTests()
    {
        _mockLogger = new Mock<ILogger<UserRepository>>();
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnActiveUsers()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new UserRepository(context, _mockLogger.Object);

        var user1 = new User { Id = 1, Name = "User1", Email = "user1@test.com", IsActive = true };
        var user2 = new User { Id = 2, Name = "User2", Email = "user2@test.com", IsActive = false };

        await context.Users.AddRangeAsync(user1, user2);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Single(result);
        Assert.Contains(result, u => u.Id == 1);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser_WhenExists()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new UserRepository(context, _mockLogger.Object);

        var user = new User { Id = 1, Name = "Test User", Email = "test@test.com", IsActive = true };
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test User", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new UserRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnUser_WhenExists()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new UserRepository(context, _mockLogger.Object);

        var user = new User { Id = 1, Name = "Test User", Email = "test@test.com", IsActive = true };
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByEmailAsync("test@test.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test@test.com", result.Email);
    }

    [Fact]
    public async Task AddAsync_ShouldAddUser()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new UserRepository(context, _mockLogger.Object);

        var user = new User
        {
            Name = "New User",
            Email = "new@test.com",
            Password = "password",
            IsActive = true
        };

        // Act
        var result = await repository.AddAsync(user);

        // Assert
        Assert.NotEqual(0, result.Id);
        Assert.Equal("New User", result.Name);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateUser()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new UserRepository(context, _mockLogger.Object);

        var user = new User { Id = 1, Name = "Old Name", Email = "test@test.com", IsActive = true };
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        context.Entry(user).State = EntityState.Detached;
        user.Name = "New Name";

        // Act
        await repository.UpdateAsync(user);

        // Assert
        var updatedUser = await context.Users.FindAsync(1);
        Assert.NotNull(updatedUser);
        Assert.Equal("New Name", updatedUser.Name);
    }

    [Fact]
    public async Task DeleteAsync_ShouldSetIsActiveToFalse()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new UserRepository(context, _mockLogger.Object);

        var user = new User { Id = 1, Name = "Test User", Email = "test@test.com", IsActive = true };
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        // Act
        await repository.DeleteAsync(1);

        // Assert
        var deletedUser = await context.Users.FindAsync(1);
        Assert.NotNull(deletedUser);
        Assert.False(deletedUser.IsActive);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenUserExists()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new UserRepository(context, _mockLogger.Object);

        var user = new User { Id = 1, Name = "Test User", Email = "test@test.com", IsActive = true };
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.ExistsAsync(1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnFalse_WhenUserDoesNotExist()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new UserRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.ExistsAsync(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task EmailExistsAsync_ShouldReturnTrue_WhenEmailExists()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new UserRepository(context, _mockLogger.Object);

        var user = new User { Id = 1, Name = "Test User", Email = "test@test.com", IsActive = true };
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.EmailExistsAsync("test@test.com");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task EmailExistsAsync_ShouldReturnFalse_WhenEmailDoesNotExist()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new UserRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.EmailExistsAsync("nonexistent@test.com");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ValidateLoginAsync_ShouldReturnUser_WhenCredentialsCorrect()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new UserRepository(context, _mockLogger.Object);

        var user = new User { Id = 1, Name = "Test User", Email = "test@test.com", Password = "password123", IsActive = true };
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.ValidateLoginAsync("test@test.com", "password123");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test@test.com", result.Email);
    }

    [Fact]
    public async Task ValidateLoginAsync_ShouldReturnNull_WhenCredentialsIncorrect()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new UserRepository(context, _mockLogger.Object);

        var user = new User { Id = 1, Name = "Test User", Email = "test@test.com", Password = "password123", IsActive = true };
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.ValidateLoginAsync("test@test.com", "wrongpassword");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnMatchingUsers()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new UserRepository(context, _mockLogger.Object);

        var user1 = new User { Id = 1, Name = "John Doe", Email = "john@test.com", PhoneNo = "123", IsActive = true };
        var user2 = new User { Id = 2, Name = "Jane Smith", Email = "jane@test.com", PhoneNo = "456", IsActive = true };

        await context.Users.AddRangeAsync(user1, user2);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.SearchAsync("John");

        // Assert
        Assert.Single(result);
        Assert.Contains(result, u => u.Name == "John Doe");
    }
}
