using Xunit;
using ClinicManagement.Domain.Enums;
using System;
using System.Linq;

namespace ClinicManagement.Domain.Tests.Enums
{
    public class GenderTests
    {
        [Fact]
        public void Gender_Male_ShouldBeDefined()
        {
            // Act
            var gender = Gender.Male;

            // Assert
            Assert.Equal(Gender.Male, gender);
        }

        [Fact]
        public void Gender_Female_ShouldBeDefined()
        {
            // Act
            var gender = Gender.Female;

            // Assert
            Assert.Equal(Gender.Female, gender);
        }

        [Fact]
        public void Gender_Other_ShouldBeDefined()
        {
            // Act
            var gender = Gender.Other;

            // Assert
            Assert.Equal(Gender.Other, gender);
        }

        [Fact]
        public void Gender_AllValues_ShouldBeAccessible()
        {
            // Arrange & Act
            var values = Enum.GetValues(typeof(Gender));

            // Assert
            Assert.Equal(3, values.Length);
            Assert.Contains(Gender.Male, values.Cast<Gender>());
            Assert.Contains(Gender.Female, values.Cast<Gender>());
            Assert.Contains(Gender.Other, values.Cast<Gender>());
        }

        [Theory]
        [InlineData(Gender.Male, "Male")]
        [InlineData(Gender.Female, "Female")]
        [InlineData(Gender.Other, "Other")]
        public void Gender_ToString_ShouldReturnCorrectName(Gender gender, string expected)
        {
            // Act
            var result = gender.ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0, Gender.Male)]
        [InlineData(1, Gender.Female)]
        [InlineData(2, Gender.Other)]
        public void Gender_CastFromInt_ShouldReturnCorrectEnum(int value, Gender expected)
        {
            // Act
            var gender = (Gender)value;

            // Assert
            Assert.Equal(expected, gender);
        }
    }
}
