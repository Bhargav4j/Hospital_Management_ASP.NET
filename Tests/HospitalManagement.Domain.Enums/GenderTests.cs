using HospitalManagement.Domain.Enums;
using Xunit;

namespace Tests.HospitalManagement.Domain.Enums;

public class GenderTests
{
    [Fact]
    public void Gender_Male_ShouldHaveValue1()
    {
        // Arrange & Act
        var result = Gender.Male;

        // Assert
        Assert.Equal(1, (int)result);
    }

    [Fact]
    public void Gender_Female_ShouldHaveValue2()
    {
        // Arrange & Act
        var result = Gender.Female;

        // Assert
        Assert.Equal(2, (int)result);
    }

    [Fact]
    public void Gender_Other_ShouldHaveValue3()
    {
        // Arrange & Act
        var result = Gender.Other;

        // Assert
        Assert.Equal(3, (int)result);
    }

    [Fact]
    public void Gender_ShouldHaveThreeValues()
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
    public void Gender_CastFromInt_ShouldReturnCorrectValue(int value, Gender expected)
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
