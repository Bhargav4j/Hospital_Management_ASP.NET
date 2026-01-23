using Xunit;
using Microsoft.EntityFrameworkCore;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Infrastructure.Data;
using System;

namespace ClinicManagement.Infrastructure.Data.Tests;

/// <summary>
/// Unit tests for ClinicDbContext
/// </summary>
public class ClinicDbContextTests
{
    private ClinicDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ClinicDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ClinicDbContext(options);
    }

    [Fact]
    public void ClinicDbContext_Constructor_ShouldInitializeDbSets()
    {
        // Arrange & Act
        using var context = CreateInMemoryContext();

        // Assert
        Assert.NotNull(context.Patients);
        Assert.NotNull(context.Doctors);
        Assert.NotNull(context.Appointments);
        Assert.NotNull(context.TreatmentHistories);
        Assert.NotNull(context.Bills);
        Assert.NotNull(context.Feedbacks);
        Assert.NotNull(context.Notifications);
        Assert.NotNull(context.Staff);
    }

    [Fact]
    public void ClinicDbContext_Patients_ShouldAllowAddAndRetrieve()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var patient = new Patient
        {
            Id = 1,
            Name = "Test Patient",
            Email = "test@test.com",
            IsActive = true
        };

        // Act
        context.Patients.Add(patient);
        context.SaveChanges();

        // Assert
        var retrieved = context.Patients.Find(1);
        Assert.NotNull(retrieved);
        Assert.Equal("Test Patient", retrieved.Name);
    }

    [Fact]
    public void ClinicDbContext_Doctors_ShouldAllowAddAndRetrieve()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var doctor = new Doctor
        {
            Id = 1,
            Name = "Dr. Test",
            Email = "doctor@test.com",
            Specialization = "General",
            IsActive = true
        };

        // Act
        context.Doctors.Add(doctor);
        context.SaveChanges();

        // Assert
        var retrieved = context.Doctors.Find(1);
        Assert.NotNull(retrieved);
        Assert.Equal("Dr. Test", retrieved.Name);
    }

    [Fact]
    public void ClinicDbContext_Appointments_ShouldAllowAddAndRetrieve()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var appointment = new Appointment
        {
            Id = 1,
            PatientId = 1,
            DoctorId = 1,
            Status = "Pending",
            IsActive = true
        };

        // Act
        context.Appointments.Add(appointment);
        context.SaveChanges();

        // Assert
        var retrieved = context.Appointments.Find(1);
        Assert.NotNull(retrieved);
        Assert.Equal("Pending", retrieved.Status);
    }

    [Fact]
    public void ClinicDbContext_TreatmentHistories_ShouldAllowAddAndRetrieve()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var treatmentHistory = new TreatmentHistory
        {
            Id = 1,
            AppointmentId = 1,
            PatientId = 1,
            DoctorId = 1,
            Diagnosis = "Test Diagnosis",
            IsActive = true
        };

        // Act
        context.TreatmentHistories.Add(treatmentHistory);
        context.SaveChanges();

        // Assert
        var retrieved = context.TreatmentHistories.Find(1);
        Assert.NotNull(retrieved);
        Assert.Equal("Test Diagnosis", retrieved.Diagnosis);
    }

    [Fact]
    public void ClinicDbContext_Bills_ShouldAllowAddAndRetrieve()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var bill = new Bill
        {
            Id = 1,
            PatientId = 1,
            AppointmentId = 1,
            TotalAmount = 100.00m,
            PaymentStatus = "Unpaid",
            IsActive = true
        };

        // Act
        context.Bills.Add(bill);
        context.SaveChanges();

        // Assert
        var retrieved = context.Bills.Find(1);
        Assert.NotNull(retrieved);
        Assert.Equal(100.00m, retrieved.TotalAmount);
    }

    [Fact]
    public void ClinicDbContext_Feedbacks_ShouldAllowAddAndRetrieve()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var feedback = new Feedback
        {
            Id = 1,
            PatientId = 1,
            Subject = "Test Feedback",
            Message = "Test Message",
            Status = "Pending",
            IsActive = true
        };

        // Act
        context.Feedbacks.Add(feedback);
        context.SaveChanges();

        // Assert
        var retrieved = context.Feedbacks.Find(1);
        Assert.NotNull(retrieved);
        Assert.Equal("Test Feedback", retrieved.Subject);
    }

    [Fact]
    public void ClinicDbContext_Notifications_ShouldAllowAddAndRetrieve()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var notification = new Notification
        {
            Id = 1,
            Title = "Test Notification",
            Message = "Test Message",
            Type = "Info",
            IsActive = true
        };

        // Act
        context.Notifications.Add(notification);
        context.SaveChanges();

        // Assert
        var retrieved = context.Notifications.Find(1);
        Assert.NotNull(retrieved);
        Assert.Equal("Test Notification", retrieved.Title);
    }

    [Fact]
    public void ClinicDbContext_Staff_ShouldAllowAddAndRetrieve()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var staff = new Staff
        {
            Id = 1,
            Name = "Test Staff",
            Email = "staff@test.com",
            Role = "Receptionist",
            IsActive = true
        };

        // Act
        context.Staff.Add(staff);
        context.SaveChanges();

        // Assert
        var retrieved = context.Staff.Find(1);
        Assert.NotNull(retrieved);
        Assert.Equal("Test Staff", retrieved.Name);
    }

    [Fact]
    public void ClinicDbContext_SaveChanges_ShouldPersistData()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var patient = new Patient
        {
            Id = 1,
            Name = "Test",
            Email = "test@test.com",
            IsActive = true
        };

        // Act
        context.Patients.Add(patient);
        var result = context.SaveChanges();

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void ClinicDbContext_Model_ShouldHaveDefaultSchema()
    {
        // Arrange & Act
        using var context = CreateInMemoryContext();

        // Assert
        Assert.NotNull(context.Model);
    }
}
