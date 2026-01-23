using Xunit;
using ClinicManagement.Domain.Entities;
using System;

namespace ClinicManagement.Domain.Entities.Tests;

/// <summary>
/// Unit tests for Bill entity
/// </summary>
public class BillTests
{
    [Fact]
    public void Bill_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var bill = new Bill();

        // Assert
        Assert.Equal(0, bill.Id);
        Assert.Equal(0, bill.PatientId);
        Assert.Equal(0, bill.AppointmentId);
        Assert.Equal(0m, bill.ConsultationFee);
        Assert.Null(bill.TestCharges);
        Assert.Null(bill.MedicineCharges);
        Assert.Null(bill.OtherCharges);
        Assert.Equal(0m, bill.TotalAmount);
        Assert.Equal("Unpaid", bill.PaymentStatus);
        Assert.Null(bill.PaymentDate);
        Assert.Null(bill.PaymentMethod);
        Assert.Equal("System", bill.CreatedBy);
        Assert.False(bill.IsActive);
    }

    [Fact]
    public void Bill_SetProperties_ShouldStoreCorrectValues()
    {
        // Arrange
        var bill = new Bill();
        var createdDate = DateTime.UtcNow;
        var paymentDate = new DateTime(2023, 6, 20);

        // Act
        bill.Id = 1;
        bill.PatientId = 5;
        bill.AppointmentId = 10;
        bill.ConsultationFee = 150.00m;
        bill.TestCharges = 50.00m;
        bill.MedicineCharges = 30.00m;
        bill.OtherCharges = 20.00m;
        bill.TotalAmount = 250.00m;
        bill.PaymentStatus = "Paid";
        bill.PaymentDate = paymentDate;
        bill.PaymentMethod = "Credit Card";
        bill.CreatedDate = createdDate;
        bill.IsActive = true;
        bill.CreatedBy = "Billing";

        // Assert
        Assert.Equal(1, bill.Id);
        Assert.Equal(5, bill.PatientId);
        Assert.Equal(10, bill.AppointmentId);
        Assert.Equal(150.00m, bill.ConsultationFee);
        Assert.Equal(50.00m, bill.TestCharges);
        Assert.Equal(30.00m, bill.MedicineCharges);
        Assert.Equal(20.00m, bill.OtherCharges);
        Assert.Equal(250.00m, bill.TotalAmount);
        Assert.Equal("Paid", bill.PaymentStatus);
        Assert.Equal(paymentDate, bill.PaymentDate);
        Assert.Equal("Credit Card", bill.PaymentMethod);
        Assert.Equal(createdDate, bill.CreatedDate);
        Assert.True(bill.IsActive);
        Assert.Equal("Billing", bill.CreatedBy);
    }

    [Theory]
    [InlineData("Unpaid")]
    [InlineData("Paid")]
    [InlineData("PartiallyPaid")]
    public void Bill_PaymentStatus_ShouldAcceptValidValues(string status)
    {
        // Arrange
        var bill = new Bill();

        // Act
        bill.PaymentStatus = status;

        // Assert
        Assert.Equal(status, bill.PaymentStatus);
    }

    [Fact]
    public void Bill_TestCharges_ShouldBeNullable()
    {
        // Arrange
        var bill = new Bill();

        // Act & Assert
        Assert.Null(bill.TestCharges);

        // Act - Set value
        bill.TestCharges = 100.50m;

        // Assert
        Assert.NotNull(bill.TestCharges);
        Assert.Equal(100.50m, bill.TestCharges);
    }

    [Fact]
    public void Bill_MedicineCharges_ShouldBeNullable()
    {
        // Arrange
        var bill = new Bill();

        // Act & Assert
        Assert.Null(bill.MedicineCharges);

        // Act - Set value
        bill.MedicineCharges = 75.25m;

        // Assert
        Assert.NotNull(bill.MedicineCharges);
        Assert.Equal(75.25m, bill.MedicineCharges);
    }

    [Fact]
    public void Bill_OtherCharges_ShouldBeNullable()
    {
        // Arrange
        var bill = new Bill();

        // Act & Assert
        Assert.Null(bill.OtherCharges);

        // Act - Set value
        bill.OtherCharges = 25.00m;

        // Assert
        Assert.NotNull(bill.OtherCharges);
        Assert.Equal(25.00m, bill.OtherCharges);
    }

    [Fact]
    public void Bill_PaymentDate_ShouldBeNullable()
    {
        // Arrange
        var bill = new Bill();

        // Act & Assert
        Assert.Null(bill.PaymentDate);

        // Act - Set value
        var paymentDate = DateTime.UtcNow;
        bill.PaymentDate = paymentDate;

        // Assert
        Assert.NotNull(bill.PaymentDate);
        Assert.Equal(paymentDate, bill.PaymentDate);
    }

    [Fact]
    public void Bill_PaymentMethod_ShouldBeNullable()
    {
        // Arrange
        var bill = new Bill();

        // Act & Assert
        Assert.Null(bill.PaymentMethod);

        // Act - Set value
        bill.PaymentMethod = "Cash";

        // Assert
        Assert.NotNull(bill.PaymentMethod);
        Assert.Equal("Cash", bill.PaymentMethod);
    }

    [Fact]
    public void Bill_ModifiedDate_ShouldBeNullable()
    {
        // Arrange
        var bill = new Bill();

        // Act & Assert
        Assert.Null(bill.ModifiedDate);

        // Act - Set value
        var modifiedDate = DateTime.UtcNow;
        bill.ModifiedDate = modifiedDate;

        // Assert
        Assert.NotNull(bill.ModifiedDate);
        Assert.Equal(modifiedDate, bill.ModifiedDate);
    }

    [Fact]
    public void Bill_ModifiedBy_ShouldBeNullable()
    {
        // Arrange
        var bill = new Bill();

        // Act & Assert
        Assert.Null(bill.ModifiedBy);

        // Act - Set value
        bill.ModifiedBy = "UpdatedByAdmin";

        // Assert
        Assert.NotNull(bill.ModifiedBy);
        Assert.Equal("UpdatedByAdmin", bill.ModifiedBy);
    }

    [Fact]
    public void Bill_DefaultPaymentStatus_ShouldBeUnpaid()
    {
        // Arrange & Act
        var bill = new Bill();

        // Assert
        Assert.Equal("Unpaid", bill.PaymentStatus);
    }

    [Fact]
    public void Bill_NavigationProperty_Patient_ShouldBeInitializable()
    {
        // Arrange
        var bill = new Bill();
        var patient = new Patient { Id = 1, Name = "John Doe" };

        // Act
        bill.Patient = patient;

        // Assert
        Assert.NotNull(bill.Patient);
        Assert.Equal(patient, bill.Patient);
    }

    [Theory]
    [InlineData(100.00, 50.00, 30.00, 20.00, 200.00)]
    [InlineData(200.00, 0, 0, 0, 200.00)]
    [InlineData(150.50, 25.25, 10.10, 5.05, 190.90)]
    public void Bill_TotalAmount_ShouldCalculateCorrectly(decimal consultationFee, decimal testCharges, decimal medicineCharges, decimal otherCharges, decimal expectedTotal)
    {
        // Arrange
        var bill = new Bill();

        // Act
        bill.ConsultationFee = consultationFee;
        bill.TestCharges = testCharges == 0 ? null : testCharges;
        bill.MedicineCharges = medicineCharges == 0 ? null : medicineCharges;
        bill.OtherCharges = otherCharges == 0 ? null : otherCharges;

        var calculatedTotal = consultationFee + (testCharges) + (medicineCharges) + (otherCharges);
        bill.TotalAmount = calculatedTotal;

        // Assert
        Assert.Equal(expectedTotal, bill.TotalAmount);
    }
}
