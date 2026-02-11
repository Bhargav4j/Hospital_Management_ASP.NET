using Xunit;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Entities.Tests;

public class BillTests
{
    [Fact]
    public void Bill_Constructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var bill = new Bill();

        // Assert
        Assert.NotNull(bill);
        Assert.Equal(0, bill.Id);
        Assert.Equal(0, bill.PatientId);
        Assert.Equal(default(DateTime), bill.BillDate);
        Assert.Equal(0m, bill.TotalAmount);
        Assert.Equal(0m, bill.PaidAmount);
        Assert.Equal(0m, bill.Balance);
        Assert.Equal(string.Empty, bill.Status);
        Assert.Null(bill.Description);
        Assert.Equal(default(DateTime), bill.CreatedDate);
        Assert.Null(bill.ModifiedDate);
        Assert.False(bill.IsActive);
        Assert.Equal(string.Empty, bill.CreatedBy);
        Assert.Null(bill.ModifiedBy);
        Assert.Null(bill.Patient);
    }

    [Fact]
    public void Bill_SetId_StoresValue()
    {
        // Arrange
        var bill = new Bill();
        var expectedId = 999;

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
        var expectedPatientId = 50;

        // Act
        bill.PatientId = expectedPatientId;

        // Assert
        Assert.Equal(expectedPatientId, bill.PatientId);
    }

    [Fact]
    public void Bill_SetBillDate_StoresValue()
    {
        // Arrange
        var bill = new Bill();
        var expectedDate = new DateTime(2025, 10, 1);

        // Act
        bill.BillDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, bill.BillDate);
    }

    [Fact]
    public void Bill_SetTotalAmount_StoresValue()
    {
        // Arrange
        var bill = new Bill();
        var expectedAmount = 1500.50m;

        // Act
        bill.TotalAmount = expectedAmount;

        // Assert
        Assert.Equal(expectedAmount, bill.TotalAmount);
    }

    [Fact]
    public void Bill_SetTotalAmount_WithZero_StoresValue()
    {
        // Arrange
        var bill = new Bill();

        // Act
        bill.TotalAmount = 0m;

        // Assert
        Assert.Equal(0m, bill.TotalAmount);
    }

    [Fact]
    public void Bill_SetPaidAmount_StoresValue()
    {
        // Arrange
        var bill = new Bill();
        var expectedAmount = 750.25m;

        // Act
        bill.PaidAmount = expectedAmount;

        // Assert
        Assert.Equal(expectedAmount, bill.PaidAmount);
    }

    [Fact]
    public void Bill_SetPaidAmount_WithZero_StoresValue()
    {
        // Arrange
        var bill = new Bill();

        // Act
        bill.PaidAmount = 0m;

        // Assert
        Assert.Equal(0m, bill.PaidAmount);
    }

    [Fact]
    public void Bill_SetBalance_StoresValue()
    {
        // Arrange
        var bill = new Bill();
        var expectedBalance = 750.25m;

        // Act
        bill.Balance = expectedBalance;

        // Assert
        Assert.Equal(expectedBalance, bill.Balance);
    }

    [Fact]
    public void Bill_SetBalance_WithZero_StoresValue()
    {
        // Arrange
        var bill = new Bill();

        // Act
        bill.Balance = 0m;

        // Assert
        Assert.Equal(0m, bill.Balance);
    }

    [Fact]
    public void Bill_SetStatus_WithPaid_StoresValue()
    {
        // Arrange
        var bill = new Bill();
        var expectedStatus = "Paid";

        // Act
        bill.Status = expectedStatus;

        // Assert
        Assert.Equal(expectedStatus, bill.Status);
    }

    [Fact]
    public void Bill_SetStatus_WithPending_StoresValue()
    {
        // Arrange
        var bill = new Bill();
        var expectedStatus = "Pending";

        // Act
        bill.Status = expectedStatus;

        // Assert
        Assert.Equal(expectedStatus, bill.Status);
    }

    [Fact]
    public void Bill_SetStatus_WithPartial_StoresValue()
    {
        // Arrange
        var bill = new Bill();
        var expectedStatus = "Partial";

        // Act
        bill.Status = expectedStatus;

        // Assert
        Assert.Equal(expectedStatus, bill.Status);
    }

    [Fact]
    public void Bill_SetDescription_WithNull_StoresValue()
    {
        // Arrange
        var bill = new Bill();

        // Act
        bill.Description = null;

        // Assert
        Assert.Null(bill.Description);
    }

    [Fact]
    public void Bill_SetDescription_WithValue_StoresValue()
    {
        // Arrange
        var bill = new Bill();
        var expectedDescription = "Consultation and lab tests";

        // Act
        bill.Description = expectedDescription;

        // Assert
        Assert.Equal(expectedDescription, bill.Description);
    }

    [Fact]
    public void Bill_SetCreatedDate_StoresValue()
    {
        // Arrange
        var bill = new Bill();
        var expectedDate = DateTime.UtcNow;

        // Act
        bill.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, bill.CreatedDate);
    }

    [Fact]
    public void Bill_SetModifiedDate_WithNull_StoresValue()
    {
        // Arrange
        var bill = new Bill();

        // Act
        bill.ModifiedDate = null;

        // Assert
        Assert.Null(bill.ModifiedDate);
    }

    [Fact]
    public void Bill_SetModifiedDate_WithValue_StoresValue()
    {
        // Arrange
        var bill = new Bill();
        var expectedDate = DateTime.UtcNow;

        // Act
        bill.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, bill.ModifiedDate);
    }

    [Fact]
    public void Bill_SetIsActive_WithTrue_StoresValue()
    {
        // Arrange
        var bill = new Bill();

        // Act
        bill.IsActive = true;

        // Assert
        Assert.True(bill.IsActive);
    }

    [Fact]
    public void Bill_SetIsActive_WithFalse_StoresValue()
    {
        // Arrange
        var bill = new Bill();

        // Act
        bill.IsActive = false;

        // Assert
        Assert.False(bill.IsActive);
    }

    [Fact]
    public void Bill_SetCreatedBy_StoresValue()
    {
        // Arrange
        var bill = new Bill();
        var expectedCreatedBy = "billing_clerk";

        // Act
        bill.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, bill.CreatedBy);
    }

    [Fact]
    public void Bill_SetModifiedBy_WithNull_StoresValue()
    {
        // Arrange
        var bill = new Bill();

        // Act
        bill.ModifiedBy = null;

        // Assert
        Assert.Null(bill.ModifiedBy);
    }

    [Fact]
    public void Bill_SetModifiedBy_WithValue_StoresValue()
    {
        // Arrange
        var bill = new Bill();
        var expectedModifiedBy = "accountant";

        // Act
        bill.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, bill.ModifiedBy);
    }

    [Fact]
    public void Bill_SetPatient_StoresValue()
    {
        // Arrange
        var bill = new Bill();
        var patient = new Patient { Id = 1, FirstName = "David", LastName = "Lee" };

        // Act
        bill.Patient = patient;

        // Assert
        Assert.NotNull(bill.Patient);
        Assert.Equal(1, bill.Patient.Id);
        Assert.Equal("David", bill.Patient.FirstName);
        Assert.Equal("Lee", bill.Patient.LastName);
    }

    [Fact]
    public void Bill_AllProperties_CanBeSetTogether()
    {
        // Arrange
        var bill = new Bill();
        var patient = new Patient { Id = 25, FirstName = "Nancy", LastName = "Taylor" };
        var billDate = new DateTime(2025, 11, 5);
        var createdDate = DateTime.UtcNow;
        var modifiedDate = DateTime.UtcNow;

        // Act
        bill.Id = 500;
        bill.PatientId = 25;
        bill.BillDate = billDate;
        bill.TotalAmount = 2500.75m;
        bill.PaidAmount = 1000.00m;
        bill.Balance = 1500.75m;
        bill.Status = "Partial";
        bill.Description = "Surgery and post-operative care";
        bill.CreatedDate = createdDate;
        bill.ModifiedDate = modifiedDate;
        bill.IsActive = true;
        bill.CreatedBy = "system";
        bill.ModifiedBy = "finance";
        bill.Patient = patient;

        // Assert
        Assert.Equal(500, bill.Id);
        Assert.Equal(25, bill.PatientId);
        Assert.Equal(billDate, bill.BillDate);
        Assert.Equal(2500.75m, bill.TotalAmount);
        Assert.Equal(1000.00m, bill.PaidAmount);
        Assert.Equal(1500.75m, bill.Balance);
        Assert.Equal("Partial", bill.Status);
        Assert.Equal("Surgery and post-operative care", bill.Description);
        Assert.Equal(createdDate, bill.CreatedDate);
        Assert.Equal(modifiedDate, bill.ModifiedDate);
        Assert.True(bill.IsActive);
        Assert.Equal("system", bill.CreatedBy);
        Assert.Equal("finance", bill.ModifiedBy);
        Assert.NotNull(bill.Patient);
    }

    [Fact]
    public void Bill_CalculateBalance_CorrectlyStoresValue()
    {
        // Arrange
        var bill = new Bill();
        bill.TotalAmount = 1000m;
        bill.PaidAmount = 400m;

        // Act
        bill.Balance = bill.TotalAmount - bill.PaidAmount;

        // Assert
        Assert.Equal(600m, bill.Balance);
    }

    [Theory]
    [InlineData(1000.00, 1000.00, 0)]
    [InlineData(2500.50, 1000.00, 1500.50)]
    [InlineData(500.00, 0, 500.00)]
    [InlineData(750.75, 750.75, 0)]
    public void Bill_Balance_CalculatedCorrectly(decimal total, decimal paid, decimal expectedBalance)
    {
        // Arrange
        var bill = new Bill();
        bill.TotalAmount = total;
        bill.PaidAmount = paid;

        // Act
        bill.Balance = bill.TotalAmount - bill.PaidAmount;

        // Assert
        Assert.Equal(expectedBalance, bill.Balance);
    }

    [Fact]
    public void Bill_SetTotalAmount_WithNegativeValue_StoresValue()
    {
        // Arrange
        var bill = new Bill();

        // Act
        bill.TotalAmount = -100m;

        // Assert
        Assert.Equal(-100m, bill.TotalAmount);
    }

    [Fact]
    public void Bill_SetPaidAmount_WithNegativeValue_StoresValue()
    {
        // Arrange
        var bill = new Bill();

        // Act
        bill.PaidAmount = -50m;

        // Assert
        Assert.Equal(-50m, bill.PaidAmount);
    }
}
