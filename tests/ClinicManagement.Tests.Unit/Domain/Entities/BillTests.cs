using Xunit;
using ClinicManagement.Domain.Entities;
using System;

namespace ClinicManagement.Tests.Unit.Domain.Entities;

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
        Assert.Null(bill.Patient);
        Assert.Equal(0, bill.AppointmentId);
        Assert.Equal(0m, bill.Amount);
        Assert.Equal(default(DateTime), bill.BillDate);
        Assert.Equal(string.Empty, bill.Status);
        Assert.Equal(default(DateTime), bill.CreatedAt);
        Assert.Null(bill.UpdatedAt);
    }

    [Fact]
    public void Bill_Id_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var bill = new Bill();
        var expectedId = 400;

        // Act
        bill.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, bill.Id);
    }

    [Fact]
    public void Bill_PatientId_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var bill = new Bill();
        var expectedPatientId = 100;

        // Act
        bill.PatientId = expectedPatientId;

        // Assert
        Assert.Equal(expectedPatientId, bill.PatientId);
    }

    [Fact]
    public void Bill_Patient_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var bill = new Bill();
        var expectedPatient = new Patient { Id = 1, UserId = 10 };

        // Act
        bill.Patient = expectedPatient;

        // Assert
        Assert.Equal(expectedPatient, bill.Patient);
        Assert.Equal(1, bill.Patient.Id);
        Assert.Equal(10, bill.Patient.UserId);
    }

    [Fact]
    public void Bill_AppointmentId_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var bill = new Bill();
        var expectedAppointmentId = 500;

        // Act
        bill.AppointmentId = expectedAppointmentId;

        // Assert
        Assert.Equal(expectedAppointmentId, bill.AppointmentId);
    }

    [Fact]
    public void Bill_Amount_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var bill = new Bill();
        var expectedAmount = 150.50m;

        // Act
        bill.Amount = expectedAmount;

        // Assert
        Assert.Equal(expectedAmount, bill.Amount);
    }

    [Fact]
    public void Bill_BillDate_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var bill = new Bill();
        var expectedBillDate = new DateTime(2024, 12, 31);

        // Act
        bill.BillDate = expectedBillDate;

        // Assert
        Assert.Equal(expectedBillDate, bill.BillDate);
    }

    [Fact]
    public void Bill_Status_ShouldSetAndGetCorrectly()
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
    public void Bill_CreatedAt_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var bill = new Bill();
        var expectedCreatedAt = DateTime.Now;

        // Act
        bill.CreatedAt = expectedCreatedAt;

        // Assert
        Assert.Equal(expectedCreatedAt, bill.CreatedAt);
    }

    [Fact]
    public void Bill_UpdatedAt_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var bill = new Bill();
        var expectedUpdatedAt = DateTime.Now;

        // Act
        bill.UpdatedAt = expectedUpdatedAt;

        // Assert
        Assert.Equal(expectedUpdatedAt, bill.UpdatedAt);
    }

    [Fact]
    public void Bill_UpdatedAt_ShouldAcceptNullValue()
    {
        // Arrange
        var bill = new Bill();

        // Act
        bill.UpdatedAt = null;

        // Assert
        Assert.Null(bill.UpdatedAt);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    [InlineData(-1)]
    public void Bill_Id_ShouldAcceptVariousValues(int id)
    {
        // Arrange
        var bill = new Bill();

        // Act
        bill.Id = id;

        // Assert
        Assert.Equal(id, bill.Id);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(100.00)]
    [InlineData(999.99)]
    [InlineData(0.01)]
    public void Bill_Amount_ShouldAcceptVariousValues(double amount)
    {
        // Arrange
        var bill = new Bill();
        var decimalAmount = (decimal)amount;

        // Act
        bill.Amount = decimalAmount;

        // Assert
        Assert.Equal(decimalAmount, bill.Amount);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Paid")]
    [InlineData("Pending")]
    [InlineData("Overdue")]
    [InlineData("Cancelled")]
    public void Bill_Status_ShouldAcceptVariousValues(string status)
    {
        // Arrange
        var bill = new Bill();

        // Act
        bill.Status = status;

        // Assert
        Assert.Equal(status, bill.Status);
    }

    [Fact]
    public void Bill_AllProperties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var bill = new Bill();
        var expectedId = 777;
        var expectedPatientId = 88;
        var expectedPatient = new Patient { Id = 88 };
        var expectedAppointmentId = 999;
        var expectedAmount = 250.75m;
        var expectedBillDate = new DateTime(2025, 6, 15);
        var expectedStatus = "Unpaid";
        var expectedCreatedAt = DateTime.Now;
        var expectedUpdatedAt = DateTime.Now.AddDays(5);

        // Act
        bill.Id = expectedId;
        bill.PatientId = expectedPatientId;
        bill.Patient = expectedPatient;
        bill.AppointmentId = expectedAppointmentId;
        bill.Amount = expectedAmount;
        bill.BillDate = expectedBillDate;
        bill.Status = expectedStatus;
        bill.CreatedAt = expectedCreatedAt;
        bill.UpdatedAt = expectedUpdatedAt;

        // Assert
        Assert.Equal(expectedId, bill.Id);
        Assert.Equal(expectedPatientId, bill.PatientId);
        Assert.Equal(expectedPatient, bill.Patient);
        Assert.Equal(expectedAppointmentId, bill.AppointmentId);
        Assert.Equal(expectedAmount, bill.Amount);
        Assert.Equal(expectedBillDate, bill.BillDate);
        Assert.Equal(expectedStatus, bill.Status);
        Assert.Equal(expectedCreatedAt, bill.CreatedAt);
        Assert.Equal(expectedUpdatedAt, bill.UpdatedAt);
    }

    [Fact]
    public void Bill_Amount_ShouldHandleNegativeValues()
    {
        // Arrange
        var bill = new Bill();
        var negativeAmount = -100.00m;

        // Act
        bill.Amount = negativeAmount;

        // Assert
        Assert.Equal(negativeAmount, bill.Amount);
        Assert.True(bill.Amount < 0);
    }

    [Fact]
    public void Bill_Amount_ShouldHandleZeroValue()
    {
        // Arrange
        var bill = new Bill();

        // Act
        bill.Amount = 0m;

        // Assert
        Assert.Equal(0m, bill.Amount);
    }

    [Fact]
    public void Bill_Amount_ShouldHandleLargeValues()
    {
        // Arrange
        var bill = new Bill();
        var largeAmount = 999999.99m;

        // Act
        bill.Amount = largeAmount;

        // Assert
        Assert.Equal(largeAmount, bill.Amount);
    }

    [Fact]
    public void Bill_BillDate_ShouldHandleFutureDates()
    {
        // Arrange
        var bill = new Bill();
        var futureDate = DateTime.Now.AddMonths(6);

        // Act
        bill.BillDate = futureDate;

        // Assert
        Assert.Equal(futureDate, bill.BillDate);
        Assert.True(bill.BillDate > DateTime.Now);
    }

    [Fact]
    public void Bill_BillDate_ShouldHandlePastDates()
    {
        // Arrange
        var bill = new Bill();
        var pastDate = DateTime.Now.AddYears(-1);

        // Act
        bill.BillDate = pastDate;

        // Assert
        Assert.Equal(pastDate, bill.BillDate);
        Assert.True(bill.BillDate < DateTime.Now);
    }

    [Fact]
    public void Bill_CreatedAtAndUpdatedAt_ShouldTrackTimestamps()
    {
        // Arrange
        var bill = new Bill();
        var createdTime = DateTime.Now;
        var updatedTime = createdTime.AddDays(1);

        // Act
        bill.CreatedAt = createdTime;
        bill.UpdatedAt = updatedTime;

        // Assert
        Assert.Equal(createdTime, bill.CreatedAt);
        Assert.Equal(updatedTime, bill.UpdatedAt);
        Assert.True(bill.UpdatedAt > bill.CreatedAt);
    }
}
