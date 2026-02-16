using Xunit;
using ClinicManagement.Domain.Entities;
using System;
using System.Linq;

namespace ClinicManagement.Domain.Tests.Entities
{
    public class DepartmentTests
    {
        [Fact]
        public void Department_Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var department = new Department();

            // Assert
            Assert.Equal(0, department.DeptNo);
            Assert.Equal(string.Empty, department.DeptName);
            Assert.Equal(string.Empty, department.Description);
            Assert.NotNull(department.Doctors);
            Assert.Empty(department.Doctors);
        }

        [Fact]
        public void Department_SetProperties_ShouldRetainValues()
        {
            // Arrange
            var department = new Department();

            // Act
            department.DeptNo = 1;
            department.DeptName = "Cardiology";
            department.Description = "Heart and cardiovascular care";

            // Assert
            Assert.Equal(1, department.DeptNo);
            Assert.Equal("Cardiology", department.DeptName);
            Assert.Equal("Heart and cardiovascular care", department.Description);
        }

        [Fact]
        public void Department_CreatedDate_ShouldBeSetAutomatically()
        {
            // Arrange & Act
            var before = DateTime.UtcNow.AddSeconds(-1);
            var department = new Department();
            var after = DateTime.UtcNow.AddSeconds(1);

            // Assert
            Assert.True(department.CreatedDate >= before);
            Assert.True(department.CreatedDate <= after);
        }

        [Fact]
        public void Department_ModifiedDate_ShouldBeNullByDefault()
        {
            // Act
            var department = new Department();

            // Assert
            Assert.Null(department.ModifiedDate);
        }

        [Fact]
        public void Department_ModifiedDate_CanBeSet()
        {
            // Arrange
            var department = new Department();
            var modifiedDate = DateTime.UtcNow;

            // Act
            department.ModifiedDate = modifiedDate;

            // Assert
            Assert.Equal(modifiedDate, department.ModifiedDate);
        }

        [Fact]
        public void Department_Doctors_CanAddDoctor()
        {
            // Arrange
            var department = new Department();
            var doctor = new Doctor { DoctorID = 1, Name = "Dr. Smith" };

            // Act
            department.Doctors.Add(doctor);

            // Assert
            Assert.Single(department.Doctors);
            Assert.Contains(doctor, department.Doctors);
        }

        [Fact]
        public void Department_Doctors_CanAddMultipleDoctors()
        {
            // Arrange
            var department = new Department();
            var doctor1 = new Doctor { DoctorID = 1, Name = "Dr. Smith" };
            var doctor2 = new Doctor { DoctorID = 2, Name = "Dr. Jones" };

            // Act
            department.Doctors.Add(doctor1);
            department.Doctors.Add(doctor2);

            // Assert
            Assert.Equal(2, department.Doctors.Count);
        }

        [Theory]
        [InlineData("Cardiology")]
        [InlineData("Neurology")]
        [InlineData("Pediatrics")]
        [InlineData("Emergency")]
        public void Department_DeptName_AcceptsDifferentValues(string deptName)
        {
            // Arrange
            var department = new Department();

            // Act
            department.DeptName = deptName;

            // Assert
            Assert.Equal(deptName, department.DeptName);
        }

        [Fact]
        public void Department_Description_CanBeEmpty()
        {
            // Arrange
            var department = new Department();

            // Act
            department.Description = "";

            // Assert
            Assert.Equal(string.Empty, department.Description);
        }

        [Fact]
        public void Department_Description_CanBeLongText()
        {
            // Arrange
            var department = new Department();
            var longDescription = new string('a', 500);

            // Act
            department.Description = longDescription;

            // Assert
            Assert.Equal(longDescription, department.Description);
        }
    }
}
