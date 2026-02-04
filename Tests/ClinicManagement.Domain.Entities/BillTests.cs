using Xunit;
using ClinicManagement.Domain.Entities;
using System;

namespace ClinicManagement.Domain.Entities.Tests;

/// <summary>
/// Test class for Bill entity
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
        Assert.Equal(0m, bill.Amount);
        Assert.False(bill.IsPaid);
        Assert.Equal("System", bill.CreatedBy);
        Assert.False(bill.IsActive);
    }

    [Fact]
    public void Bill_SetProperties_ShouldStoreValues()
    {
        // Arrange
        var bill = new Bill();
        var expectedId = 1;
        var expectedPatientId = 10;
        var expectedAppointmentId = 20;
        var expectedAmount = 150.75m;
        var expectedBillDate = new DateTime(2024, 11, 20);
        var expectedIsPaid = true;
        var expectedPaymentMethod = "Credit Card";
        var expectedNotes = "Payment received";
        var expectedCreatedDate = DateTime.UtcNow;
        var expectedModifiedDate = DateTime.UtcNow;
        var expectedIsActive = true;
        var expectedCreatedBy = "Admin";
        var expectedModifiedBy = "Admin";

        // Act
        bill.Id = expectedId;
        bill.PatientId = expectedPatientId;
        bill.AppointmentId = expectedAppointmentId;
        bill.Amount = expectedAmount;
        bill.BillDate = expectedBillDate;
        bill.IsPaid = expectedIsPaid;
        bill.PaymentMethod = expectedPaymentMethod;
        bill.Notes = expectedNotes;
        bill.CreatedDate = expectedCreatedDate;
        bill.ModifiedDate = expectedModifiedDate;
        bill.IsActive = expectedIsActive;
        bill.CreatedBy = expectedCreatedBy;
        bill.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedId, bill.Id);
        Assert.Equal(expectedPatientId, bill.PatientId);
        Assert.Equal(expectedAppointmentId, bill.AppointmentId);
        Assert.Equal(expectedAmount, bill.Amount);
        Assert.Equal(expectedBillDate, bill.BillDate);
        Assert.Equal(expectedIsPaid, bill.IsPaid);
        Assert.Equal(expectedPaymentMethod, bill.PaymentMethod);
        Assert.Equal(expectedNotes, bill.Notes);
        Assert.Equal(expectedCreatedDate, bill.CreatedDate);
        Assert.Equal(expectedModifiedDate, bill.ModifiedDate);
        Assert.Equal(expectedIsActive, bill.IsActive);
        Assert.Equal(expectedCreatedBy, bill.CreatedBy);
        Assert.Equal(expectedModifiedBy, bill.ModifiedBy);
    }

    [Fact]
    public void Bill_PaymentMethod_ShouldAcceptNull()
    {
        // Arrange
        var bill = new Bill();

        // Act
        bill.PaymentMethod = null;

        // Assert
        Assert.Null(bill.PaymentMethod);
    }

    [Fact]
    public void Bill_Notes_ShouldAcceptNull()
    {
        // Arrange
        var bill = new Bill();

        // Act
        bill.Notes = null;

        // Assert
        Assert.Null(bill.Notes);
    }

    [Fact]
    public void Bill_ModifiedDate_ShouldAcceptNull()
    {
        // Arrange
        var bill = new Bill();

        // Act
        bill.ModifiedDate = null;

        // Assert
        Assert.Null(bill.ModifiedDate);
    }

    [Fact]
    public void Bill_ModifiedBy_ShouldAcceptNull()
    {
        // Arrange
        var bill = new Bill();

        // Act
        bill.ModifiedBy = null;

        // Assert
        Assert.Null(bill.ModifiedBy);
    }

    [Fact]
    public void Bill_Patient_ShouldStorePatientReference()
    {
        // Arrange
        var bill = new Bill();
        var patient = new Patient { Id = 1, Name = "John Doe" };

        // Act
        bill.Patient = patient;

        // Assert
        Assert.NotNull(bill.Patient);
        Assert.Equal(patient.Id, bill.Patient.Id);
        Assert.Equal(patient.Name, bill.Patient.Name);
    }

    [Fact]
    public void Bill_IsPaid_ShouldToggleBetweenTrueAndFalse()
    {
        // Arrange
        var bill = new Bill();

        // Act & Assert
        bill.IsPaid = true;
        Assert.True(bill.IsPaid);

        bill.IsPaid = false;
        Assert.False(bill.IsPaid);
    }

    [Fact]
    public void Bill_IsActive_ShouldToggleBetweenTrueAndFalse()
    {
        // Arrange
        var bill = new Bill();

        // Act & Assert
        bill.IsActive = true;
        Assert.True(bill.IsActive);

        bill.IsActive = false;
        Assert.False(bill.IsActive);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(50.00)]
    [InlineData(100.50)]
    [InlineData(999.99)]
    public void Bill_Amount_ShouldStoreDecimalValue(decimal amount)
    {
        // Arrange
        var bill = new Bill();

        // Act
        bill.Amount = amount;

        // Assert
        Assert.Equal(amount, bill.Amount);
    }

    [Fact]
    public void Bill_BillDate_ShouldStoreDateTimeValue()
    {
        // Arrange
        var bill = new Bill();
        var billDate = new DateTime(2024, 10, 15);

        // Act
        bill.BillDate = billDate;

        // Assert
        Assert.Equal(billDate, bill.BillDate);
    }

    [Theory]
    [InlineData("Cash")]
    [InlineData("Credit Card")]
    [InlineData("Debit Card")]
    [InlineData("Insurance")]
    public void Bill_PaymentMethod_ShouldStorePaymentMethodString(string paymentMethod)
    {
        // Arrange
        var bill = new Bill();

        // Act
        bill.PaymentMethod = paymentMethod;

        // Assert
        Assert.Equal(paymentMethod, bill.PaymentMethod);
    }
}
