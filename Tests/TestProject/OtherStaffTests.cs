using Xunit;
using ClinicManagement.Domain.Entities;
using System;

namespace ClinicManagement.Domain.Tests.Entities
{
    public class OtherStaffTests
    {
        [Fact]
        public void OtherStaff_Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var staff = new OtherStaff();

            // Assert
            Assert.Equal(0, staff.StaffID);
            Assert.Equal(string.Empty, staff.Name);
            Assert.Equal(string.Empty, staff.Phone);
            Assert.Equal(string.Empty, staff.Address);
            Assert.Equal(string.Empty, staff.Gender);
            Assert.Equal(string.Empty, staff.Designation);
            Assert.Equal(0, staff.Salary);
            Assert.Equal(string.Empty, staff.Qualification);
        }

        [Fact]
        public void OtherStaff_SetProperties_ShouldRetainValues()
        {
            // Arrange
            var staff = new OtherStaff();
            var testDate = new DateTime(1985, 7, 10);

            // Act
            staff.StaffID = 1;
            staff.Name = "Jane Nurse";
            staff.Phone = "5551234567";
            staff.Address = "789 Hospital Rd";
            staff.BirthDate = testDate;
            staff.Gender = "Female";
            staff.Designation = "Nurse";
            staff.Salary = 45000.00m;
            staff.Qualification = "RN";

            // Assert
            Assert.Equal(1, staff.StaffID);
            Assert.Equal("Jane Nurse", staff.Name);
            Assert.Equal("5551234567", staff.Phone);
            Assert.Equal("789 Hospital Rd", staff.Address);
            Assert.Equal(testDate, staff.BirthDate);
            Assert.Equal("Female", staff.Gender);
            Assert.Equal("Nurse", staff.Designation);
            Assert.Equal(45000.00m, staff.Salary);
            Assert.Equal("RN", staff.Qualification);
        }

        [Fact]
        public void OtherStaff_CreatedDate_ShouldBeSetAutomatically()
        {
            // Arrange & Act
            var before = DateTime.UtcNow.AddSeconds(-1);
            var staff = new OtherStaff();
            var after = DateTime.UtcNow.AddSeconds(1);

            // Assert
            Assert.True(staff.CreatedDate >= before);
            Assert.True(staff.CreatedDate <= after);
        }

        [Fact]
        public void OtherStaff_ModifiedDate_ShouldBeNullByDefault()
        {
            // Act
            var staff = new OtherStaff();

            // Assert
            Assert.Null(staff.ModifiedDate);
        }

        [Fact]
        public void OtherStaff_ModifiedDate_CanBeSet()
        {
            // Arrange
            var staff = new OtherStaff();
            var modifiedDate = DateTime.UtcNow;

            // Act
            staff.ModifiedDate = modifiedDate;

            // Assert
            Assert.Equal(modifiedDate, staff.ModifiedDate);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(30000.00)]
        [InlineData(50000.50)]
        [InlineData(75000.99)]
        public void OtherStaff_Salary_AcceptsDifferentValues(decimal salary)
        {
            // Arrange
            var staff = new OtherStaff();

            // Act
            staff.Salary = salary;

            // Assert
            Assert.Equal(salary, staff.Salary);
        }

        [Theory]
        [InlineData("Nurse")]
        [InlineData("Receptionist")]
        [InlineData("Technician")]
        [InlineData("Administrator")]
        public void OtherStaff_Designation_AcceptsDifferentValues(string designation)
        {
            // Arrange
            var staff = new OtherStaff();

            // Act
            staff.Designation = designation;

            // Assert
            Assert.Equal(designation, staff.Designation);
        }

        [Theory]
        [InlineData("Male")]
        [InlineData("Female")]
        [InlineData("Other")]
        public void OtherStaff_Gender_AcceptsDifferentValues(string gender)
        {
            // Arrange
            var staff = new OtherStaff();

            // Act
            staff.Gender = gender;

            // Assert
            Assert.Equal(gender, staff.Gender);
        }

        [Fact]
        public void OtherStaff_BirthDate_CanBeSetToAnyDate()
        {
            // Arrange
            var staff = new OtherStaff();
            var birthDate = new DateTime(1990, 1, 1);

            // Act
            staff.BirthDate = birthDate;

            // Assert
            Assert.Equal(birthDate, staff.BirthDate);
        }
    }
}
