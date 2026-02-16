using Xunit;
using ClinicManagement.Domain.Enums;

namespace ClinicManagement.Domain.Tests.Enums
{
    public class UserRoleTests
    {
        [Fact]
        public void UserRole_PatientValue_ShouldBe1()
        {
            // Arrange & Act
            var patientRole = UserRole.Patient;

            // Assert
            Assert.Equal(1, (int)patientRole);
        }

        [Fact]
        public void UserRole_DoctorValue_ShouldBe2()
        {
            // Arrange & Act
            var doctorRole = UserRole.Doctor;

            // Assert
            Assert.Equal(2, (int)doctorRole);
        }

        [Fact]
        public void UserRole_AdminValue_ShouldBe3()
        {
            // Arrange & Act
            var adminRole = UserRole.Admin;

            // Assert
            Assert.Equal(3, (int)adminRole);
        }

        [Fact]
        public void UserRole_AllValues_ShouldBeDefined()
        {
            // Arrange & Act
            var values = System.Enum.GetValues(typeof(UserRole));

            // Assert
            Assert.Equal(3, values.Length);
            Assert.Contains(UserRole.Patient, values.Cast<UserRole>());
            Assert.Contains(UserRole.Doctor, values.Cast<UserRole>());
            Assert.Contains(UserRole.Admin, values.Cast<UserRole>());
        }

        [Theory]
        [InlineData(1, UserRole.Patient)]
        [InlineData(2, UserRole.Doctor)]
        [InlineData(3, UserRole.Admin)]
        public void UserRole_CastFromInt_ShouldReturnCorrectEnum(int value, UserRole expected)
        {
            // Act
            var role = (UserRole)value;

            // Assert
            Assert.Equal(expected, role);
        }
    }
}
