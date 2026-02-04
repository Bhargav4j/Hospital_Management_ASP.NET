using Xunit;
using ClinicManagement.Domain.Enums;

namespace ClinicManagement.Domain.Enums.Tests;

/// <summary>
/// Test class for Gender enum
/// </summary>
public class GenderTests
{
    [Fact]
    public void Gender_Male_ShouldHaveValue1()
    {
        // Arrange & Act
        var maleValue = (int)Gender.Male;

        // Assert
        Assert.Equal(1, maleValue);
    }

    [Fact]
    public void Gender_Female_ShouldHaveValue2()
    {
        // Arrange & Act
        var femaleValue = (int)Gender.Female;

        // Assert
        Assert.Equal(2, femaleValue);
    }

    [Fact]
    public void Gender_Other_ShouldHaveValue3()
    {
        // Arrange & Act
        var otherValue = (int)Gender.Other;

        // Assert
        Assert.Equal(3, otherValue);
    }

    [Fact]
    public void Gender_ShouldContainThreeValues()
    {
        // Arrange & Act
        var values = System.Enum.GetValues(typeof(Gender));

        // Assert
        Assert.Equal(3, values.Length);
    }

    [Theory]
    [InlineData(1, Gender.Male)]
    [InlineData(2, Gender.Female)]
    [InlineData(3, Gender.Other)]
    public void Gender_CastFromInt_ShouldReturnCorrectEnum(int value, Gender expected)
    {
        // Arrange & Act
        var result = (Gender)value;

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Gender_ToString_ShouldReturnEnumName()
    {
        // Arrange
        var gender = Gender.Male;

        // Act
        var result = gender.ToString();

        // Assert
        Assert.Equal("Male", result);
    }
}
