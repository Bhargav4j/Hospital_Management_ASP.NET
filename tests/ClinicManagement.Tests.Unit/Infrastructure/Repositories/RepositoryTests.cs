using Xunit;
using Moq;
using ClinicManagement.Infrastructure.Repositories;
using ClinicManagement.Infrastructure.Data;
using ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ClinicManagement.Tests.Unit.Infrastructure.Repositories;

public class RepositoryTests
{
    private DbContextOptions<ClinicDbContext> CreateInMemoryOptions()
    {
        return new DbContextOptionsBuilder<ClinicDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnEntity()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);
        var repository = new Repository<User>(context);

        var user = new User { Id = 1, Name = "Test User", Email = "test@example.com" };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test User", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);
        var repository = new Repository<User>(context);

        // Act
        var result = await repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEntities()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);
        var repository = new Repository<User>(context);

        context.Users.AddRange(
            new User { Id = 1, Name = "User 1", Email = "user1@example.com" },
            new User { Id = 2, Name = "User 2", Email = "user2@example.com" },
            new User { Id = 3, Name = "User 3", Email = "user3@example.com" }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task GetAllAsync_WithEmptyDatabase_ShouldReturnEmptyList()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);
        var repository = new Repository<User>(context);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task FindAsync_WithMatchingPredicate_ShouldReturnFilteredEntities()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);
        var repository = new Repository<User>(context);

        context.Users.AddRange(
            new User { Id = 1, Name = "John Doe", Email = "john@example.com", Type = 1 },
            new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com", Type = 2 },
            new User { Id = 3, Name = "Bob Johnson", Email = "bob@example.com", Type = 1 }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await repository.FindAsync(u => u.Type == 1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, u => Assert.Equal(1, u.Type));
    }

    [Fact]
    public async Task FindAsync_WithNoMatches_ShouldReturnEmptyList()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);
        var repository = new Repository<User>(context);

        context.Users.Add(new User { Id = 1, Name = "Test", Email = "test@example.com", Type = 1 });
        await context.SaveChangesAsync();

        // Act
        var result = await repository.FindAsync(u => u.Type == 99);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntityAndReturnIt()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);
        var repository = new Repository<User>(context);

        var newUser = new User
        {
            Name = "New User",
            Email = "new@example.com",
            Type = 1
        };

        // Act
        var result = await repository.AddAsync(newUser);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New User", result.Name);
        Assert.True(result.Id > 0);

        var savedUser = await context.Users.FindAsync(result.Id);
        Assert.NotNull(savedUser);
        Assert.Equal("New User", savedUser.Name);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateExistingEntity()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);
        var repository = new Repository<User>(context);

        var user = new User { Id = 1, Name = "Original Name", Email = "original@example.com" };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Act
        user.Name = "Updated Name";
        await repository.UpdateAsync(user);

        // Assert
        var updatedUser = await context.Users.FindAsync(1);
        Assert.NotNull(updatedUser);
        Assert.Equal("Updated Name", updatedUser.Name);
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ShouldRemoveEntity()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);
        var repository = new Repository<User>(context);

        var user = new User { Id = 1, Name = "To Delete", Email = "delete@example.com" };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Act
        await repository.DeleteAsync(1);

        // Assert
        var deletedUser = await context.Users.FindAsync(1);
        Assert.Null(deletedUser);
    }

    [Fact]
    public async Task DeleteAsync_WithInvalidId_ShouldNotThrowException()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);
        var repository = new Repository<User>(context);

        // Act & Assert - should not throw
        await repository.DeleteAsync(999);
    }

    [Fact]
    public async Task CountAsync_WithoutPredicate_ShouldReturnTotalCount()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);
        var repository = new Repository<User>(context);

        context.Users.AddRange(
            new User { Id = 1, Name = "User 1", Email = "user1@example.com" },
            new User { Id = 2, Name = "User 2", Email = "user2@example.com" },
            new User { Id = 3, Name = "User 3", Email = "user3@example.com" }
        );
        await context.SaveChangesAsync();

        // Act
        var count = await repository.CountAsync();

        // Assert
        Assert.Equal(3, count);
    }

    [Fact]
    public async Task CountAsync_WithPredicate_ShouldReturnFilteredCount()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);
        var repository = new Repository<User>(context);

        context.Users.AddRange(
            new User { Id = 1, Name = "User 1", Email = "user1@example.com", Type = 1 },
            new User { Id = 2, Name = "User 2", Email = "user2@example.com", Type = 2 },
            new User { Id = 3, Name = "User 3", Email = "user3@example.com", Type = 1 }
        );
        await context.SaveChangesAsync();

        // Act
        var count = await repository.CountAsync(u => u.Type == 1);

        // Assert
        Assert.Equal(2, count);
    }

    [Fact]
    public async Task CountAsync_WithEmptyDatabase_ShouldReturnZero()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);
        var repository = new Repository<User>(context);

        // Act
        var count = await repository.CountAsync();

        // Assert
        Assert.Equal(0, count);
    }

    [Fact]
    public async Task CountAsync_WithNoMatchingPredicate_ShouldReturnZero()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);
        var repository = new Repository<User>(context);

        context.Users.Add(new User { Id = 1, Name = "User", Email = "user@example.com", Type = 1 });
        await context.SaveChangesAsync();

        // Act
        var count = await repository.CountAsync(u => u.Type == 99);

        // Assert
        Assert.Equal(0, count);
    }

    [Fact]
    public async Task AddAsync_MultipleTimes_ShouldAddAllEntities()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);
        var repository = new Repository<User>(context);

        // Act
        await repository.AddAsync(new User { Name = "User 1", Email = "user1@example.com" });
        await repository.AddAsync(new User { Name = "User 2", Email = "user2@example.com" });
        await repository.AddAsync(new User { Name = "User 3", Email = "user3@example.com" });

        // Assert
        var count = await repository.CountAsync();
        Assert.Equal(3, count);
    }

    [Fact]
    public async Task FindAsync_WithComplexPredicate_ShouldReturnCorrectEntities()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);
        var repository = new Repository<User>(context);

        context.Users.AddRange(
            new User { Id = 1, Name = "Alice", Email = "alice@example.com", Type = 1 },
            new User { Id = 2, Name = "Bob", Email = "bob@example.com", Type = 2 },
            new User { Id = 3, Name = "Charlie", Email = "charlie@example.com", Type = 1 }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await repository.FindAsync(u => u.Type == 1 && u.Name.StartsWith("A"));

        // Assert
        Assert.Single(result);
        Assert.Equal("Alice", result.First().Name);
    }

    [Fact]
    public async Task Repository_WithDifferentEntityTypes_ShouldWorkCorrectly()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);
        var userRepository = new Repository<User>(context);
        var patientRepository = new Repository<Patient>(context);

        // Act
        await userRepository.AddAsync(new User { Name = "User", Email = "user@example.com" });
        await patientRepository.AddAsync(new Patient { UserId = 1, MedicalHistory = "History" });

        // Assert
        var userCount = await userRepository.CountAsync();
        var patientCount = await patientRepository.CountAsync();

        Assert.Equal(1, userCount);
        Assert.Equal(1, patientCount);
    }
}
