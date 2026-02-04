using Xunit;
using ClinicManagement.Domain.Enums;

namespace ClinicManagement.Domain.Enums.Tests;

/// <summary>
/// Test class for AppointmentStatus enum
/// </summary>
public class AppointmentStatusTests
{
    [Fact]
    public void AppointmentStatus_Pending_ShouldHaveValue1()
    {
        // Arrange & Act
        var pendingValue = (int)AppointmentStatus.Pending;

        // Assert
        Assert.Equal(1, pendingValue);
    }

    [Fact]
    public void AppointmentStatus_Approved_ShouldHaveValue2()
    {
        // Arrange & Act
        var approvedValue = (int)AppointmentStatus.Approved;

        // Assert
        Assert.Equal(2, approvedValue);
    }

    [Fact]
    public void AppointmentStatus_Rejected_ShouldHaveValue3()
    {
        // Arrange & Act
        var rejectedValue = (int)AppointmentStatus.Rejected;

        // Assert
        Assert.Equal(3, rejectedValue);
    }

    [Fact]
    public void AppointmentStatus_Completed_ShouldHaveValue4()
    {
        // Arrange & Act
        var completedValue = (int)AppointmentStatus.Completed;

        // Assert
        Assert.Equal(4, completedValue);
    }

    [Fact]
    public void AppointmentStatus_Cancelled_ShouldHaveValue5()
    {
        // Arrange & Act
        var cancelledValue = (int)AppointmentStatus.Cancelled;

        // Assert
        Assert.Equal(5, cancelledValue);
    }

    [Fact]
    public void AppointmentStatus_ShouldContainFiveValues()
    {
        // Arrange & Act
        var values = System.Enum.GetValues(typeof(AppointmentStatus));

        // Assert
        Assert.Equal(5, values.Length);
    }

    [Theory]
    [InlineData(1, AppointmentStatus.Pending)]
    [InlineData(2, AppointmentStatus.Approved)]
    [InlineData(3, AppointmentStatus.Rejected)]
    [InlineData(4, AppointmentStatus.Completed)]
    [InlineData(5, AppointmentStatus.Cancelled)]
    public void AppointmentStatus_CastFromInt_ShouldReturnCorrectEnum(int value, AppointmentStatus expected)
    {
        // Arrange & Act
        var result = (AppointmentStatus)value;

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void AppointmentStatus_ToString_ShouldReturnEnumName()
    {
        // Arrange
        var status = AppointmentStatus.Pending;

        // Act
        var result = status.ToString();

        // Assert
        Assert.Equal("Pending", result);
    }
}
