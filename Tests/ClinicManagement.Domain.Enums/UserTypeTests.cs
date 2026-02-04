using Xunit;
using ClinicManagement.Domain.Enums;

namespace ClinicManagement.Domain.Enums.Tests;

/// <summary>
/// Test class for UserType enum
/// </summary>
public class UserTypeTests
{
    [Fact]
    public void UserType_Patient_ShouldHaveValue1()
    {
        // Arrange & Act
        var patientValue = (int)UserType.Patient;

        // Assert
        Assert.Equal(1, patientValue);
    }

    [Fact]
    public void UserType_Doctor_ShouldHaveValue2()
    {
        // Arrange & Act
        var doctorValue = (int)UserType.Doctor;

        // Assert
        Assert.Equal(2, doctorValue);
    }

    [Fact]
    public void UserType_Admin_ShouldHaveValue3()
    {
        // Arrange & Act
        var adminValue = (int)UserType.Admin;

        // Assert
        Assert.Equal(3, adminValue);
    }

    [Fact]
    public void UserType_Staff_ShouldHaveValue4()
    {
        // Arrange & Act
        var staffValue = (int)UserType.Staff;

        // Assert
        Assert.Equal(4, staffValue);
    }

    [Fact]
    public void UserType_ShouldContainFourValues()
    {
        // Arrange & Act
        var values = System.Enum.GetValues(typeof(UserType));

        // Assert
        Assert.Equal(4, values.Length);
    }

    [Theory]
    [InlineData(1, UserType.Patient)]
    [InlineData(2, UserType.Doctor)]
    [InlineData(3, UserType.Admin)]
    [InlineData(4, UserType.Staff)]
    public void UserType_CastFromInt_ShouldReturnCorrectEnum(int value, UserType expected)
    {
        // Arrange & Act
        var result = (UserType)value;

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void UserType_ToString_ShouldReturnEnumName()
    {
        // Arrange
        var userType = UserType.Patient;

        // Act
        var result = userType.ToString();

        // Assert
        Assert.Equal("Patient", result);
    }
}
