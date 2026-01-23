using System;
using Xunit;
using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Entities.Tests
{
    public class BillTests
    {
        [Fact]
        public void Bill_Constructor_InitializesProperties()
        {
            // Arrange & Act
            var bill = new Bill();

            // Assert
            Assert.NotNull(bill);
            Assert.Equal(0, bill.Id);
            Assert.Equal(0, bill.PatientId);
            Assert.Equal(0m, bill.Amount);
            Assert.Equal(string.Empty, bill.Status);
            Assert.Equal(string.Empty, bill.CreatedBy);
            Assert.False(bill.IsActive);
        }

        [Fact]
        public void Bill_SetId_StoresValue()
        {
            // Arrange
            var bill = new Bill();
            int expectedId = 301;

            // Act
            bill.Id = expectedId;

            // Assert
            Assert.Equal(expectedId, bill.Id);
        }

        [Fact]
        public void Bill_SetPatientId_StoresValue()
        {
            // Arrange
            var bill = new Bill();
            int expectedPatientId = 150;

            // Act
            bill.PatientId = expectedPatientId;

            // Assert
            Assert.Equal(expectedPatientId, bill.PatientId);
        }

        [Fact]
        public void Bill_SetDoctorId_StoresValue()
        {
            // Arrange
            var bill = new Bill();
            int? expectedDoctorId = 75;

            // Act
            bill.DoctorId = expectedDoctorId;

            // Assert
            Assert.Equal(expectedDoctorId, bill.DoctorId);
        }

        [Fact]
        public void Bill_SetDoctorId_AcceptsNull()
        {
            // Arrange
            var bill = new Bill();

            // Act
            bill.DoctorId = null;

            // Assert
            Assert.Null(bill.DoctorId);
        }

        [Fact]
        public void Bill_SetAmount_StoresValue()
        {
            // Arrange
            var bill = new Bill();
            decimal expectedAmount = 1250.50m;

            // Act
            bill.Amount = expectedAmount;

            // Assert
            Assert.Equal(expectedAmount, bill.Amount);
        }

        [Fact]
        public void Bill_SetAmount_StoresZero()
        {
            // Arrange
            var bill = new Bill();

            // Act
            bill.Amount = 0m;

            // Assert
            Assert.Equal(0m, bill.Amount);
        }

        [Fact]
        public void Bill_SetBillDate_StoresValue()
        {
            // Arrange
            var bill = new Bill();
            DateTime expectedDate = new DateTime(2026, 1, 20);

            // Act
            bill.BillDate = expectedDate;

            // Assert
            Assert.Equal(expectedDate, bill.BillDate);
        }

        [Fact]
        public void Bill_SetStatus_StoresValue()
        {
            // Arrange
            var bill = new Bill();
            string expectedStatus = "Paid";

            // Act
            bill.Status = expectedStatus;

            // Assert
            Assert.Equal(expectedStatus, bill.Status);
        }

        [Fact]
        public void Bill_SetDescription_StoresValue()
        {
            // Arrange
            var bill = new Bill();
            string expectedDescription = "Consultation and medication";

            // Act
            bill.Description = expectedDescription;

            // Assert
            Assert.Equal(expectedDescription, bill.Description);
        }

        [Fact]
        public void Bill_SetDescription_AcceptsNull()
        {
            // Arrange
            var bill = new Bill();

            // Act
            bill.Description = null;

            // Assert
            Assert.Null(bill.Description);
        }

        [Fact]
        public void Bill_SetCreatedDate_StoresValue()
        {
            // Arrange
            var bill = new Bill();
            DateTime expectedDate = DateTime.UtcNow;

            // Act
            bill.CreatedDate = expectedDate;

            // Assert
            Assert.Equal(expectedDate, bill.CreatedDate);
        }

        [Fact]
        public void Bill_SetModifiedDate_StoresValue()
        {
            // Arrange
            var bill = new Bill();
            DateTime? expectedDate = DateTime.UtcNow;

            // Act
            bill.ModifiedDate = expectedDate;

            // Assert
            Assert.Equal(expectedDate, bill.ModifiedDate);
        }

        [Fact]
        public void Bill_SetModifiedDate_AcceptsNull()
        {
            // Arrange
            var bill = new Bill();

            // Act
            bill.ModifiedDate = null;

            // Assert
            Assert.Null(bill.ModifiedDate);
        }

        [Fact]
        public void Bill_SetIsActive_StoresTrue()
        {
            // Arrange
            var bill = new Bill();

            // Act
            bill.IsActive = true;

            // Assert
            Assert.True(bill.IsActive);
        }

        [Fact]
        public void Bill_SetCreatedBy_StoresValue()
        {
            // Arrange
            var bill = new Bill();
            string expectedCreatedBy = "BillingSystem";

            // Act
            bill.CreatedBy = expectedCreatedBy;

            // Assert
            Assert.Equal(expectedCreatedBy, bill.CreatedBy);
        }

        [Fact]
        public void Bill_SetModifiedBy_StoresValue()
        {
            // Arrange
            var bill = new Bill();
            string expectedModifiedBy = "Accountant";

            // Act
            bill.ModifiedBy = expectedModifiedBy;

            // Assert
            Assert.Equal(expectedModifiedBy, bill.ModifiedBy);
        }

        [Fact]
        public void Bill_SetModifiedBy_AcceptsNull()
        {
            // Arrange
            var bill = new Bill();

            // Act
            bill.ModifiedBy = null;

            // Assert
            Assert.Null(bill.ModifiedBy);
        }

        [Fact]
        public void Bill_AllProperties_CanBeSetAndRetrieved()
        {
            // Arrange
            var bill = new Bill
            {
                Id = 999,
                PatientId = 500,
                DoctorId = 250,
                Amount = 2500.75m,
                BillDate = new DateTime(2026, 2, 1),
                Status = "Pending",
                Description = "Emergency surgery and follow-up",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                IsActive = true,
                CreatedBy = "System",
                ModifiedBy = "Finance"
            };

            // Assert
            Assert.Equal(999, bill.Id);
            Assert.Equal(500, bill.PatientId);
            Assert.Equal(250, bill.DoctorId);
            Assert.Equal(2500.75m, bill.Amount);
            Assert.Equal(new DateTime(2026, 2, 1), bill.BillDate);
            Assert.Equal("Pending", bill.Status);
            Assert.Equal("Emergency surgery and follow-up", bill.Description);
            Assert.True(bill.IsActive);
            Assert.Equal("System", bill.CreatedBy);
            Assert.Equal("Finance", bill.ModifiedBy);
        }
    }
}
