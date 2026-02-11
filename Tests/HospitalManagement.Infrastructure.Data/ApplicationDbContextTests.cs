using HospitalManagement.Domain.Entities;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.HospitalManagement.Infrastructure.Data;

public class ApplicationDbContextTests
{
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

    public ApplicationDbContextTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void ApplicationDbContext_Constructor_ShouldInitializeDbSets()
    {
        // Arrange & Act
        using var context = new ApplicationDbContext(_dbContextOptions);

        // Assert
        Assert.NotNull(context.Users);
        Assert.NotNull(context.Appointments);
        Assert.NotNull(context.Bills);
        Assert.NotNull(context.Feedbacks);
        Assert.NotNull(context.MedicalHistories);
        Assert.NotNull(context.Clinics);
    }

    [Fact]
    public async Task ApplicationDbContext_CanAddAndRetrieveUser()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var user = new User
        {
            Name = "Test User",
            Email = "test@test.com",
            Password = "password",
            IsActive = true
        };

        // Act
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var retrievedUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "test@test.com");

        // Assert
        Assert.NotNull(retrievedUser);
        Assert.Equal("Test User", retrievedUser.Name);
    }

    [Fact]
    public async Task ApplicationDbContext_CanAddAndRetrieveAppointment()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var patient = new User { Name = "Patient", Email = "patient@test.com", IsActive = true };
        var doctor = new User { Name = "Doctor", Email = "doctor@test.com", IsActive = true };

        await context.Users.AddRangeAsync(patient, doctor);
        await context.SaveChangesAsync();

        var appointment = new Appointment
        {
            PatientId = patient.Id,
            DoctorId = doctor.Id,
            AppointmentDate = DateTime.UtcNow,
            IsActive = true
        };

        // Act
        await context.Appointments.AddAsync(appointment);
        await context.SaveChangesAsync();

        var retrievedAppointment = await context.Appointments.FirstOrDefaultAsync(a => a.PatientId == patient.Id);

        // Assert
        Assert.NotNull(retrievedAppointment);
        Assert.Equal(patient.Id, retrievedAppointment.PatientId);
    }

    [Fact]
    public async Task ApplicationDbContext_CanAddAndRetrieveBill()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var patient = new User { Name = "Patient", Email = "patient@test.com", IsActive = true };

        await context.Users.AddAsync(patient);
        await context.SaveChangesAsync();

        var bill = new Bill
        {
            PatientId = patient.Id,
            Amount = 100.00m,
            Description = "Test Bill",
            IsActive = true
        };

        // Act
        await context.Bills.AddAsync(bill);
        await context.SaveChangesAsync();

        var retrievedBill = await context.Bills.FirstOrDefaultAsync(b => b.PatientId == patient.Id);

        // Assert
        Assert.NotNull(retrievedBill);
        Assert.Equal(100.00m, retrievedBill.Amount);
    }

    [Fact]
    public async Task ApplicationDbContext_CanAddAndRetrieveFeedback()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var patient = new User { Name = "Patient", Email = "patient@test.com", IsActive = true };

        await context.Users.AddAsync(patient);
        await context.SaveChangesAsync();

        var feedback = new Feedback
        {
            PatientId = patient.Id,
            Message = "Great service!",
            Rating = 5,
            IsActive = true
        };

        // Act
        await context.Feedbacks.AddAsync(feedback);
        await context.SaveChangesAsync();

        var retrievedFeedback = await context.Feedbacks.FirstOrDefaultAsync(f => f.PatientId == patient.Id);

        // Assert
        Assert.NotNull(retrievedFeedback);
        Assert.Equal("Great service!", retrievedFeedback.Message);
    }

    [Fact]
    public async Task ApplicationDbContext_CanAddAndRetrieveMedicalHistory()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var patient = new User { Name = "Patient", Email = "patient@test.com", IsActive = true };

        await context.Users.AddAsync(patient);
        await context.SaveChangesAsync();

        var medicalHistory = new MedicalHistory
        {
            PatientId = patient.Id,
            Diagnosis = "Flu",
            Treatment = "Rest",
            VisitDate = DateTime.UtcNow,
            IsActive = true
        };

        // Act
        await context.MedicalHistories.AddAsync(medicalHistory);
        await context.SaveChangesAsync();

        var retrievedHistory = await context.MedicalHistories.FirstOrDefaultAsync(m => m.PatientId == patient.Id);

        // Assert
        Assert.NotNull(retrievedHistory);
        Assert.Equal("Flu", retrievedHistory.Diagnosis);
    }

    [Fact]
    public async Task ApplicationDbContext_CanAddAndRetrieveClinic()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var clinic = new Clinic
        {
            Name = "Test Clinic",
            Address = "123 Medical St",
            PhoneNo = "555-1234",
            IsActive = true
        };

        // Act
        await context.Clinics.AddAsync(clinic);
        await context.SaveChangesAsync();

        var retrievedClinic = await context.Clinics.FirstOrDefaultAsync(c => c.Name == "Test Clinic");

        // Assert
        Assert.NotNull(retrievedClinic);
        Assert.Equal("123 Medical St", retrievedClinic.Address);
    }
}
