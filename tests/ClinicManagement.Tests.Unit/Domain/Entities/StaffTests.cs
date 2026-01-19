using Xunit;
using ClinicManagement.Domain.Entities;
using System;

namespace ClinicManagement.Tests.Unit.Domain.Entities;

public class StaffTests
{
    [Fact]
    public void Staff_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var staff = new Staff();

        // Assert
        Assert.Equal(0, staff.Id);
        Assert.Equal(0, staff.UserId);
        Assert.Null(staff.User);
        Assert.Equal(string.Empty, staff.Position);
        Assert.Equal(default(DateTime), staff.CreatedAt);
        Assert.Null(staff.UpdatedAt);
    }

    [Fact]
    public void Staff_Id_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var staff = new Staff();
        var expectedId = 500;

        // Act
        staff.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, staff.Id);
    }

    [Fact]
    public void Staff_UserId_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var staff = new Staff();
        var expectedUserId = 250;

        // Act
        staff.UserId = expectedUserId;

        // Assert
        Assert.Equal(expectedUserId, staff.UserId);
    }

    [Fact]
    public void Staff_User_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var staff = new Staff();
        var expectedUser = new User { Id = 1, Name = "John Staff" };

        // Act
        staff.User = expectedUser;

        // Assert
        Assert.Equal(expectedUser, staff.User);
        Assert.Equal(1, staff.User.Id);
        Assert.Equal("John Staff", staff.User.Name);
    }

    [Fact]
    public void Staff_Position_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var staff = new Staff();
        var expectedPosition = "Nurse";

        // Act
        staff.Position = expectedPosition;

        // Assert
        Assert.Equal(expectedPosition, staff.Position);
    }

    [Fact]
    public void Staff_CreatedAt_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var staff = new Staff();
        var expectedCreatedAt = DateTime.Now;

        // Act
        staff.CreatedAt = expectedCreatedAt;

        // Assert
        Assert.Equal(expectedCreatedAt, staff.CreatedAt);
    }

    [Fact]
    public void Staff_UpdatedAt_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var staff = new Staff();
        var expectedUpdatedAt = DateTime.Now;

        // Act
        staff.UpdatedAt = expectedUpdatedAt;

        // Assert
        Assert.Equal(expectedUpdatedAt, staff.UpdatedAt);
    }

    [Fact]
    public void Staff_UpdatedAt_ShouldAcceptNullValue()
    {
        // Arrange
        var staff = new Staff();

        // Act
        staff.UpdatedAt = null;

        // Assert
        Assert.Null(staff.UpdatedAt);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    [InlineData(-1)]
    public void Staff_Id_ShouldAcceptVariousValues(int id)
    {
        // Arrange
        var staff = new Staff();

        // Act
        staff.Id = id;

        // Assert
        Assert.Equal(id, staff.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Receptionist")]
    [InlineData("Nurse")]
    [InlineData("Administrator")]
    [InlineData("Lab Technician")]
    public void Staff_Position_ShouldAcceptVariousValues(string position)
    {
        // Arrange
        var staff = new Staff();

        // Act
        staff.Position = position;

        // Assert
        Assert.Equal(position, staff.Position);
    }

    [Fact]
    public void Staff_AllProperties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var staff = new Staff();
        var expectedId = 123;
        var expectedUserId = 456;
        var expectedUser = new User { Id = 456, Name = "Jane Staff" };
        var expectedPosition = "Manager";
        var expectedCreatedAt = DateTime.Now;
        var expectedUpdatedAt = DateTime.Now.AddHours(1);

        // Act
        staff.Id = expectedId;
        staff.UserId = expectedUserId;
        staff.User = expectedUser;
        staff.Position = expectedPosition;
        staff.CreatedAt = expectedCreatedAt;
        staff.UpdatedAt = expectedUpdatedAt;

        // Assert
        Assert.Equal(expectedId, staff.Id);
        Assert.Equal(expectedUserId, staff.UserId);
        Assert.Equal(expectedUser, staff.User);
        Assert.Equal(expectedPosition, staff.Position);
        Assert.Equal(expectedCreatedAt, staff.CreatedAt);
        Assert.Equal(expectedUpdatedAt, staff.UpdatedAt);
    }

    [Fact]
    public void Staff_CreatedAtAndUpdatedAt_ShouldTrackTimestamps()
    {
        // Arrange
        var staff = new Staff();
        var createdTime = DateTime.Now;
        var updatedTime = createdTime.AddMinutes(30);

        // Act
        staff.CreatedAt = createdTime;
        staff.UpdatedAt = updatedTime;

        // Assert
        Assert.Equal(createdTime, staff.CreatedAt);
        Assert.Equal(updatedTime, staff.UpdatedAt);
        Assert.True(staff.UpdatedAt > staff.CreatedAt);
    }

    [Fact]
    public void Staff_User_ShouldAllowReassignment()
    {
        // Arrange
        var staff = new Staff();
        var firstUser = new User { Id = 1, Name = "First User" };
        var secondUser = new User { Id = 2, Name = "Second User" };

        // Act
        staff.User = firstUser;
        Assert.Equal(firstUser, staff.User);

        staff.User = secondUser;

        // Assert
        Assert.Equal(secondUser, staff.User);
        Assert.NotEqual(firstUser, staff.User);
    }

    [Fact]
    public void Staff_Position_ShouldAllowEmptyString()
    {
        // Arrange
        var staff = new Staff();

        // Act
        staff.Position = string.Empty;

        // Assert
        Assert.Equal(string.Empty, staff.Position);
        Assert.Empty(staff.Position);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(100, 200)]
    public void Staff_IdAndUserId_ShouldBeIndependent(int id, int userId)
    {
        // Arrange
        var staff = new Staff();

        // Act
        staff.Id = id;
        staff.UserId = userId;

        // Assert
        Assert.Equal(id, staff.Id);
        Assert.Equal(userId, staff.UserId);
    }
}
