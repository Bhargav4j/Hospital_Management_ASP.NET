using System;
using Xunit;
using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Entities.Tests
{
    public class StaffTests
    {
        [Fact]
        public void Staff_Constructor_InitializesProperties()
        {
            // Arrange & Act
            var staff = new Staff();

            // Assert
            Assert.NotNull(staff);
            Assert.Equal(0, staff.Id);
            Assert.Equal(string.Empty, staff.Name);
            Assert.Equal(string.Empty, staff.Email);
            Assert.Equal(string.Empty, staff.Password);
            Assert.Equal(string.Empty, staff.Phone);
            Assert.Equal(string.Empty, staff.Role);
            Assert.Equal(string.Empty, staff.CreatedBy);
            Assert.False(staff.IsActive);
        }

        [Fact]
        public void Staff_SetId_StoresValue()
        {
            // Arrange
            var staff = new Staff();
            int expectedId = 601;

            // Act
            staff.Id = expectedId;

            // Assert
            Assert.Equal(expectedId, staff.Id);
        }

        [Fact]
        public void Staff_SetName_StoresValue()
        {
            // Arrange
            var staff = new Staff();
            string expectedName = "Alice Johnson";

            // Act
            staff.Name = expectedName;

            // Assert
            Assert.Equal(expectedName, staff.Name);
        }

        [Fact]
        public void Staff_SetEmail_StoresValue()
        {
            // Arrange
            var staff = new Staff();
            string expectedEmail = "alice@clinic.com";

            // Act
            staff.Email = expectedEmail;

            // Assert
            Assert.Equal(expectedEmail, staff.Email);
        }

        [Fact]
        public void Staff_SetPassword_StoresValue()
        {
            // Arrange
            var staff = new Staff();
            string expectedPassword = "StaffPass456";

            // Act
            staff.Password = expectedPassword;

            // Assert
            Assert.Equal(expectedPassword, staff.Password);
        }

        [Fact]
        public void Staff_SetPhone_StoresValue()
        {
            // Arrange
            var staff = new Staff();
            string expectedPhone = "8887776655";

            // Act
            staff.Phone = expectedPhone;

            // Assert
            Assert.Equal(expectedPhone, staff.Phone);
        }

        [Fact]
        public void Staff_SetRole_StoresValue()
        {
            // Arrange
            var staff = new Staff();
            string expectedRole = "Nurse";

            // Act
            staff.Role = expectedRole;

            // Assert
            Assert.Equal(expectedRole, staff.Role);
        }

        [Fact]
        public void Staff_SetDepartment_StoresValue()
        {
            // Arrange
            var staff = new Staff();
            string expectedDepartment = "Emergency";

            // Act
            staff.Department = expectedDepartment;

            // Assert
            Assert.Equal(expectedDepartment, staff.Department);
        }

        [Fact]
        public void Staff_SetDepartment_AcceptsNull()
        {
            // Arrange
            var staff = new Staff();

            // Act
            staff.Department = null;

            // Assert
            Assert.Null(staff.Department);
        }

        [Fact]
        public void Staff_SetCreatedDate_StoresValue()
        {
            // Arrange
            var staff = new Staff();
            DateTime expectedDate = DateTime.UtcNow;

            // Act
            staff.CreatedDate = expectedDate;

            // Assert
            Assert.Equal(expectedDate, staff.CreatedDate);
        }

        [Fact]
        public void Staff_SetModifiedDate_StoresValue()
        {
            // Arrange
            var staff = new Staff();
            DateTime? expectedDate = DateTime.UtcNow;

            // Act
            staff.ModifiedDate = expectedDate;

            // Assert
            Assert.Equal(expectedDate, staff.ModifiedDate);
        }

        [Fact]
        public void Staff_SetModifiedDate_AcceptsNull()
        {
            // Arrange
            var staff = new Staff();

            // Act
            staff.ModifiedDate = null;

            // Assert
            Assert.Null(staff.ModifiedDate);
        }

        [Fact]
        public void Staff_SetIsActive_StoresTrue()
        {
            // Arrange
            var staff = new Staff();

            // Act
            staff.IsActive = true;

            // Assert
            Assert.True(staff.IsActive);
        }

        [Fact]
        public void Staff_SetIsActive_StoresFalse()
        {
            // Arrange
            var staff = new Staff();

            // Act
            staff.IsActive = false;

            // Assert
            Assert.False(staff.IsActive);
        }

        [Fact]
        public void Staff_SetCreatedBy_StoresValue()
        {
            // Arrange
            var staff = new Staff();
            string expectedCreatedBy = "HR";

            // Act
            staff.CreatedBy = expectedCreatedBy;

            // Assert
            Assert.Equal(expectedCreatedBy, staff.CreatedBy);
        }

        [Fact]
        public void Staff_SetModifiedBy_StoresValue()
        {
            // Arrange
            var staff = new Staff();
            string expectedModifiedBy = "Manager";

            // Act
            staff.ModifiedBy = expectedModifiedBy;

            // Assert
            Assert.Equal(expectedModifiedBy, staff.ModifiedBy);
        }

        [Fact]
        public void Staff_SetModifiedBy_AcceptsNull()
        {
            // Arrange
            var staff = new Staff();

            // Act
            staff.ModifiedBy = null;

            // Assert
            Assert.Null(staff.ModifiedBy);
        }

        [Fact]
        public void Staff_AllProperties_CanBeSetAndRetrieved()
        {
            // Arrange
            var staff = new Staff
            {
                Id = 888,
                Name = "Bob Miller",
                Email = "bob@hospital.com",
                Password = "SecureStaff789",
                Phone = "7778889999",
                Role = "Receptionist",
                Department = "General",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                IsActive = true,
                CreatedBy = "Admin",
                ModifiedBy = "Supervisor"
            };

            // Assert
            Assert.Equal(888, staff.Id);
            Assert.Equal("Bob Miller", staff.Name);
            Assert.Equal("bob@hospital.com", staff.Email);
            Assert.Equal("SecureStaff789", staff.Password);
            Assert.Equal("7778889999", staff.Phone);
            Assert.Equal("Receptionist", staff.Role);
            Assert.Equal("General", staff.Department);
            Assert.True(staff.IsActive);
            Assert.Equal("Admin", staff.CreatedBy);
            Assert.Equal("Supervisor", staff.ModifiedBy);
        }
    }
}
