using HospitalManagement.Domain.Enums;
using Xunit;

namespace Tests.HospitalManagement.Domain.Enums;

public class UserTypeTests
{
    [Fact]
    public void UserType_Patient_ShouldHaveValue1()
    {
        // Arrange & Act
        var result = UserType.Patient;

        // Assert
        Assert.Equal(1, (int)result);
    }

    [Fact]
    public void UserType_Doctor_ShouldHaveValue2()
    {
        // Arrange & Act
        var result = UserType.Doctor;

        // Assert
        Assert.Equal(2, (int)result);
    }

    [Fact]
    public void UserType_Admin_ShouldHaveValue3()
    {
        // Arrange & Act
        var result = UserType.Admin;

        // Assert
        Assert.Equal(3, (int)result);
    }

    [Fact]
    public void UserType_ShouldHaveThreeValues()
    {
        // Arrange & Act
        var values = System.Enum.GetValues(typeof(UserType));

        // Assert
        Assert.Equal(3, values.Length);
    }

    [Theory]
    [InlineData(1, UserType.Patient)]
    [InlineData(2, UserType.Doctor)]
    [InlineData(3, UserType.Admin)]
    public void UserType_CastFromInt_ShouldReturnCorrectValue(int value, UserType expected)
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
