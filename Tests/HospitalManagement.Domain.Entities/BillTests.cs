using HospitalManagement.Domain.Entities;
using Xunit;

namespace Tests.HospitalManagement.Domain.Entities;

public class BillTests
{
    [Fact]
    public void Bill_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var bill = new Bill();

        // Assert
        Assert.Equal(0, bill.Id);
        Assert.Equal(0, bill.PatientId);
        Assert.Equal(0m, bill.Amount);
        Assert.Equal(string.Empty, bill.Description);
        Assert.False(bill.IsPaid);
        Assert.Equal("System", bill.CreatedBy);
        Assert.False(bill.IsActive);
    }

    [Fact]
    public void Bill_SetProperties_ShouldSetValues()
    {
        // Arrange
        var bill = new Bill();
        var billDate = DateTime.UtcNow;
        var createdDate = DateTime.UtcNow;

        // Act
        bill.Id = 1;
        bill.PatientId = 10;
        bill.Amount = 150.75m;
        bill.BillDate = billDate;
        bill.Description = "Consultation fee";
        bill.IsPaid = true;
        bill.CreatedDate = createdDate;
        bill.IsActive = true;

        // Assert
        Assert.Equal(1, bill.Id);
        Assert.Equal(10, bill.PatientId);
        Assert.Equal(150.75m, bill.Amount);
        Assert.Equal(billDate, bill.BillDate);
        Assert.Equal("Consultation fee", bill.Description);
        Assert.True(bill.IsPaid);
        Assert.Equal(createdDate, bill.CreatedDate);
        Assert.True(bill.IsActive);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(50.00)]
    [InlineData(100.50)]
    [InlineData(999.99)]
    public void Bill_Amount_CanBeSetToValidAmounts(decimal amount)
    {
        // Arrange
        var bill = new Bill();

        // Act
        bill.Amount = amount;

        // Assert
        Assert.Equal(amount, bill.Amount);
    }

    [Fact]
    public void Bill_IsPaid_DefaultsToFalse()
    {
        // Arrange & Act
        var bill = new Bill();

        // Assert
        Assert.False(bill.IsPaid);
    }

    [Fact]
    public void Bill_IsPaid_CanBeSetToTrue()
    {
        // Arrange
        var bill = new Bill();

        // Act
        bill.IsPaid = true;

        // Assert
        Assert.True(bill.IsPaid);
    }

    [Fact]
    public void Bill_Description_CanBeEmpty()
    {
        // Arrange & Act
        var bill = new Bill { Description = string.Empty };

        // Assert
        Assert.Equal(string.Empty, bill.Description);
    }

    [Fact]
    public void Bill_ModifiedDate_CanBeNull()
    {
        // Arrange & Act
        var bill = new Bill();

        // Assert
        Assert.Null(bill.ModifiedDate);
    }

    [Fact]
    public void Bill_ModifiedDate_CanBeSet()
    {
        // Arrange
        var bill = new Bill();
        var modifiedDate = DateTime.UtcNow;

        // Act
        bill.ModifiedDate = modifiedDate;

        // Assert
        Assert.Equal(modifiedDate, bill.ModifiedDate);
    }

    [Fact]
    public void Bill_ModifiedBy_CanBeNull()
    {
        // Arrange & Act
        var bill = new Bill();

        // Assert
        Assert.Null(bill.ModifiedBy);
    }

    [Fact]
    public void Bill_ModifiedBy_CanBeSet()
    {
        // Arrange
        var bill = new Bill();

        // Act
        bill.ModifiedBy = "Admin";

        // Assert
        Assert.Equal("Admin", bill.ModifiedBy);
    }

    [Fact]
    public void Bill_Patient_CanBeSet()
    {
        // Arrange
        var bill = new Bill();
        var patient = new User { Id = 10, Name = "Patient Name" };

        // Act
        bill.Patient = patient;

        // Assert
        Assert.NotNull(bill.Patient);
        Assert.Equal(10, bill.Patient.Id);
    }

    [Fact]
    public void Bill_CreatedBy_DefaultsToSystem()
    {
        // Arrange & Act
        var bill = new Bill();

        // Assert
        Assert.Equal("System", bill.CreatedBy);
    }
}
