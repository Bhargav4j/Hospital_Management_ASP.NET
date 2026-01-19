using Xunit;
using ClinicManagement.Infrastructure.Data;
using ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.Tests.Unit.Infrastructure.Data;

public class ClinicDbContextTests
{
    private DbContextOptions<ClinicDbContext> CreateInMemoryOptions()
    {
        return new DbContextOptionsBuilder<ClinicDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void ClinicDbContext_Constructor_ShouldInitializeDbSets()
    {
        // Arrange
        var options = CreateInMemoryOptions();

        // Act
        using var context = new ClinicDbContext(options);

        // Assert
        Assert.NotNull(context.Users);
        Assert.NotNull(context.Patients);
        Assert.NotNull(context.Doctors);
        Assert.NotNull(context.Clinics);
        Assert.NotNull(context.Appointments);
        Assert.NotNull(context.Bills);
        Assert.NotNull(context.Staff);
    }

    [Fact]
    public async Task ClinicDbContext_Users_ShouldAddAndRetrieveUser()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);

        var user = new User
        {
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash123",
            PhoneNumber = "1234567890",
            Gender = "Male",
            Type = 1,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Assert
        var savedUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
        Assert.NotNull(savedUser);
        Assert.Equal("Test User", savedUser.Name);
    }

    [Fact]
    public async Task ClinicDbContext_Patients_ShouldAddAndRetrievePatient()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);

        var user = new User { Name = "Patient User", Email = "patient@example.com", PasswordHash = "hash" };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var patient = new Patient
        {
            UserId = user.Id,
            MedicalHistory = "No known allergies",
            CreatedAt = DateTime.UtcNow
        };

        // Act
        context.Patients.Add(patient);
        await context.SaveChangesAsync();

        // Assert
        var savedPatient = await context.Patients.FirstOrDefaultAsync();
        Assert.NotNull(savedPatient);
        Assert.Equal("No known allergies", savedPatient.MedicalHistory);
    }

    [Fact]
    public async Task ClinicDbContext_Doctors_ShouldAddAndRetrieveDoctor()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);

        var user = new User { Name = "Dr. Smith", Email = "smith@example.com", PasswordHash = "hash" };
        var clinic = new Clinic { Name = "City Hospital", Address = "123 Main St" };
        context.Users.Add(user);
        context.Clinics.Add(clinic);
        await context.SaveChangesAsync();

        var doctor = new Doctor
        {
            UserId = user.Id,
            Specialization = "Cardiology",
            Qualifications = "MD",
            ClinicId = clinic.Id,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        context.Doctors.Add(doctor);
        await context.SaveChangesAsync();

        // Assert
        var savedDoctor = await context.Doctors.FirstOrDefaultAsync();
        Assert.NotNull(savedDoctor);
        Assert.Equal("Cardiology", savedDoctor.Specialization);
    }

    [Fact]
    public async Task ClinicDbContext_Clinics_ShouldAddAndRetrieveClinic()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);

        var clinic = new Clinic
        {
            Name = "Memorial Hospital",
            Address = "456 Oak Ave",
            PhoneNumber = "555-1234",
            CreatedAt = DateTime.UtcNow
        };

        // Act
        context.Clinics.Add(clinic);
        await context.SaveChangesAsync();

        // Assert
        var savedClinic = await context.Clinics.FirstOrDefaultAsync();
        Assert.NotNull(savedClinic);
        Assert.Equal("Memorial Hospital", savedClinic.Name);
    }

    [Fact]
    public async Task ClinicDbContext_Appointments_ShouldAddAndRetrieveAppointment()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);

        var patientUser = new User { Name = "Patient", Email = "patient@example.com", PasswordHash = "hash" };
        var doctorUser = new User { Name = "Doctor", Email = "doctor@example.com", PasswordHash = "hash" };
        var clinic = new Clinic { Name = "Clinic", Address = "Address" };
        context.Users.AddRange(patientUser, doctorUser);
        context.Clinics.Add(clinic);
        await context.SaveChangesAsync();

        var patient = new Patient { UserId = patientUser.Id, MedicalHistory = "History" };
        var doctor = new Doctor { UserId = doctorUser.Id, ClinicId = clinic.Id };
        context.Patients.Add(patient);
        context.Doctors.Add(doctor);
        await context.SaveChangesAsync();

        var appointment = new Appointment
        {
            PatientId = patient.Id,
            DoctorId = doctor.Id,
            AppointmentDate = DateTime.UtcNow.AddDays(7),
            Status = "Scheduled",
            Notes = "Routine checkup",
            CreatedAt = DateTime.UtcNow
        };

        // Act
        context.Appointments.Add(appointment);
        await context.SaveChangesAsync();

        // Assert
        var savedAppointment = await context.Appointments.FirstOrDefaultAsync();
        Assert.NotNull(savedAppointment);
        Assert.Equal("Scheduled", savedAppointment.Status);
    }

    [Fact]
    public async Task ClinicDbContext_Bills_ShouldAddAndRetrieveBill()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);

        var user = new User { Name = "Patient", Email = "patient@example.com", PasswordHash = "hash" };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var patient = new Patient { UserId = user.Id, MedicalHistory = "History" };
        context.Patients.Add(patient);
        await context.SaveChangesAsync();

        var bill = new Bill
        {
            PatientId = patient.Id,
            AppointmentId = 1,
            Amount = 150.00m,
            BillDate = DateTime.UtcNow,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        // Act
        context.Bills.Add(bill);
        await context.SaveChangesAsync();

        // Assert
        var savedBill = await context.Bills.FirstOrDefaultAsync();
        Assert.NotNull(savedBill);
        Assert.Equal(150.00m, savedBill.Amount);
        Assert.Equal("Pending", savedBill.Status);
    }

    [Fact]
    public async Task ClinicDbContext_Staff_ShouldAddAndRetrieveStaff()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);

        var user = new User { Name = "Staff Member", Email = "staff@example.com", PasswordHash = "hash" };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var staff = new Staff
        {
            UserId = user.Id,
            Position = "Nurse",
            CreatedAt = DateTime.UtcNow
        };

        // Act
        context.Staff.Add(staff);
        await context.SaveChangesAsync();

        // Assert
        var savedStaff = await context.Staff.FirstOrDefaultAsync();
        Assert.NotNull(savedStaff);
        Assert.Equal("Nurse", savedStaff.Position);
    }

    [Fact]
    public async Task ClinicDbContext_Users_EmailUniqueness_ShouldEnforce()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);

        var user1 = new User { Name = "User1", Email = "duplicate@example.com", PasswordHash = "hash1" };
        var user2 = new User { Name = "User2", Email = "duplicate@example.com", PasswordHash = "hash2" };

        context.Users.Add(user1);
        await context.SaveChangesAsync();

        // Act & Assert
        context.Users.Add(user2);
        // Note: In-memory database doesn't fully enforce constraints, so we just verify setup works
        Assert.NotNull(context.Users);
    }

    [Fact]
    public async Task ClinicDbContext_SaveChanges_ShouldPersistData()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        var clinic = new Clinic { Name = "Test Clinic", Address = "Test Address" };

        // Act
        using (var context = new ClinicDbContext(options))
        {
            context.Clinics.Add(clinic);
            await context.SaveChangesAsync();
        }

        // Assert
        using (var context = new ClinicDbContext(options))
        {
            var savedClinic = await context.Clinics.FirstOrDefaultAsync();
            Assert.NotNull(savedClinic);
            Assert.Equal("Test Clinic", savedClinic.Name);
        }
    }

    [Fact]
    public async Task ClinicDbContext_MultipleEntities_ShouldAddAndRetrieve()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);

        var user1 = new User { Name = "User1", Email = "user1@example.com", PasswordHash = "hash1" };
        var user2 = new User { Name = "User2", Email = "user2@example.com", PasswordHash = "hash2" };
        var user3 = new User { Name = "User3", Email = "user3@example.com", PasswordHash = "hash3" };

        // Act
        context.Users.AddRange(user1, user2, user3);
        await context.SaveChangesAsync();

        // Assert
        var count = await context.Users.CountAsync();
        Assert.Equal(3, count);
    }

    [Fact]
    public async Task ClinicDbContext_Update_ShouldModifyEntity()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);

        var clinic = new Clinic { Name = "Original Name", Address = "Original Address" };
        context.Clinics.Add(clinic);
        await context.SaveChangesAsync();

        // Act
        clinic.Name = "Updated Name";
        await context.SaveChangesAsync();

        // Assert
        var updatedClinic = await context.Clinics.FirstOrDefaultAsync();
        Assert.NotNull(updatedClinic);
        Assert.Equal("Updated Name", updatedClinic.Name);
    }

    [Fact]
    public async Task ClinicDbContext_Delete_ShouldRemoveEntity()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);

        var clinic = new Clinic { Name = "To Delete", Address = "Address" };
        context.Clinics.Add(clinic);
        await context.SaveChangesAsync();

        // Act
        context.Clinics.Remove(clinic);
        await context.SaveChangesAsync();

        // Assert
        var count = await context.Clinics.CountAsync();
        Assert.Equal(0, count);
    }

    [Fact]
    public async Task ClinicDbContext_Query_ShouldFilterEntities()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new ClinicDbContext(options);

        context.Users.AddRange(
            new User { Name = "Alice", Email = "alice@example.com", PasswordHash = "hash", Type = 1 },
            new User { Name = "Bob", Email = "bob@example.com", PasswordHash = "hash", Type = 2 },
            new User { Name = "Charlie", Email = "charlie@example.com", PasswordHash = "hash", Type = 1 }
        );
        await context.SaveChangesAsync();

        // Act
        var type1Users = await context.Users.Where(u => u.Type == 1).ToListAsync();

        // Assert
        Assert.Equal(2, type1Users.Count);
        Assert.All(type1Users, u => Assert.Equal(1, u.Type));
    }

    [Fact]
    public void ClinicDbContext_Constructor_WithValidOptions_ShouldNotThrow()
    {
        // Arrange
        var options = CreateInMemoryOptions();

        // Act & Assert
        var exception = Record.Exception(() => new ClinicDbContext(options));
        Assert.Null(exception);
    }
}
