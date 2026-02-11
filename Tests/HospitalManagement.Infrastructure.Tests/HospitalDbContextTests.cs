using Xunit;
using Microsoft.EntityFrameworkCore;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Infrastructure.Data;

namespace HospitalManagement.Infrastructure.Data.Tests;

public class HospitalDbContextTests
{
    private readonly DbContextOptions<HospitalDbContext> _options;

    public HospitalDbContextTests()
    {
        _options = new DbContextOptionsBuilder<HospitalDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void HospitalDbContext_Constructor_WithValidOptions_CreatesInstance()
    {
        // Arrange & Act
        using var context = new HospitalDbContext(_options);

        // Assert
        Assert.NotNull(context);
    }

    [Fact]
    public void HospitalDbContext_Doctors_ReturnsDbSet()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);

        // Act
        var doctors = context.Doctors;

        // Assert
        Assert.NotNull(doctors);
    }

    [Fact]
    public void HospitalDbContext_Patients_ReturnsDbSet()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);

        // Act
        var patients = context.Patients;

        // Assert
        Assert.NotNull(patients);
    }

    [Fact]
    public void HospitalDbContext_Appointments_ReturnsDbSet()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);

        // Act
        var appointments = context.Appointments;

        // Assert
        Assert.NotNull(appointments);
    }

    [Fact]
    public void HospitalDbContext_Departments_ReturnsDbSet()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);

        // Act
        var departments = context.Departments;

        // Assert
        Assert.NotNull(departments);
    }

    [Fact]
    public void HospitalDbContext_Treatments_ReturnsDbSet()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);

        // Act
        var treatments = context.Treatments;

        // Assert
        Assert.NotNull(treatments);
    }

    [Fact]
    public void HospitalDbContext_Bills_ReturnsDbSet()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);

        // Act
        var bills = context.Bills;

        // Assert
        Assert.NotNull(bills);
    }

    [Fact]
    public async Task HospitalDbContext_CanAddDoctor()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var doctor = new Doctor
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@test.com",
            Password = "hashedpassword",
            IsActive = true
        };

        // Act
        context.Doctors.Add(doctor);
        await context.SaveChangesAsync();

        // Assert
        var savedDoctor = await context.Doctors.FirstOrDefaultAsync(d => d.Email == "john.doe@test.com");
        Assert.NotNull(savedDoctor);
        Assert.Equal("John", savedDoctor.FirstName);
    }

    [Fact]
    public async Task HospitalDbContext_CanAddPatient()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var patient = new Patient
        {
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@test.com",
            Password = "hashedpassword",
            IsActive = true
        };

        // Act
        context.Patients.Add(patient);
        await context.SaveChangesAsync();

        // Assert
        var savedPatient = await context.Patients.FirstOrDefaultAsync(p => p.Email == "jane.smith@test.com");
        Assert.NotNull(savedPatient);
        Assert.Equal("Jane", savedPatient.FirstName);
    }

    [Fact]
    public async Task HospitalDbContext_CanAddDepartment()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var department = new Department
        {
            Name = "Cardiology",
            Description = "Heart care",
            IsActive = true
        };

        // Act
        context.Departments.Add(department);
        await context.SaveChangesAsync();

        // Assert
        var savedDepartment = await context.Departments.FirstOrDefaultAsync(d => d.Name == "Cardiology");
        Assert.NotNull(savedDepartment);
        Assert.Equal("Heart care", savedDepartment.Description);
    }

    [Fact]
    public async Task HospitalDbContext_CanAddAppointment()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var appointment = new Appointment
        {
            PatientId = 1,
            DoctorId = 1,
            Status = "Pending",
            IsActive = true
        };

        // Act
        context.Appointments.Add(appointment);
        await context.SaveChangesAsync();

        // Assert
        var savedAppointment = await context.Appointments.FirstOrDefaultAsync(a => a.Status == "Pending");
        Assert.NotNull(savedAppointment);
        Assert.Equal(1, savedAppointment.PatientId);
    }

    [Fact]
    public async Task HospitalDbContext_CanAddTreatment()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var treatment = new Treatment
        {
            PatientId = 1,
            DoctorId = 1,
            Diagnosis = "Common Cold",
            Prescription = "Rest and fluids",
            IsActive = true
        };

        // Act
        context.Treatments.Add(treatment);
        await context.SaveChangesAsync();

        // Assert
        var savedTreatment = await context.Treatments.FirstOrDefaultAsync(t => t.Diagnosis == "Common Cold");
        Assert.NotNull(savedTreatment);
        Assert.Equal("Rest and fluids", savedTreatment.Prescription);
    }

    [Fact]
    public async Task HospitalDbContext_CanAddBill()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var bill = new Bill
        {
            PatientId = 1,
            TotalAmount = 1000.00m,
            PaidAmount = 500.00m,
            Balance = 500.00m,
            Status = "Partial",
            IsActive = true
        };

        // Act
        context.Bills.Add(bill);
        await context.SaveChangesAsync();

        // Assert
        var savedBill = await context.Bills.FirstOrDefaultAsync(b => b.Status == "Partial");
        Assert.NotNull(savedBill);
        Assert.Equal(1000.00m, savedBill.TotalAmount);
    }

    [Fact]
    public async Task HospitalDbContext_CanUpdateDoctor()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var doctor = new Doctor
        {
            FirstName = "Alice",
            LastName = "Johnson",
            Email = "alice@test.com",
            Password = "pass",
            IsActive = true
        };

        context.Doctors.Add(doctor);
        await context.SaveChangesAsync();

        // Act
        doctor.FirstName = "Alicia";
        context.Doctors.Update(doctor);
        await context.SaveChangesAsync();

        // Assert
        var updatedDoctor = await context.Doctors.FindAsync(doctor.Id);
        Assert.NotNull(updatedDoctor);
        Assert.Equal("Alicia", updatedDoctor.FirstName);
    }

    [Fact]
    public async Task HospitalDbContext_CanDeleteDoctor()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var doctor = new Doctor
        {
            FirstName = "Bob",
            LastName = "Williams",
            Email = "bob@test.com",
            Password = "pass",
            IsActive = true
        };

        context.Doctors.Add(doctor);
        await context.SaveChangesAsync();

        // Act
        context.Doctors.Remove(doctor);
        await context.SaveChangesAsync();

        // Assert
        var deletedDoctor = await context.Doctors.FindAsync(doctor.Id);
        Assert.Null(deletedDoctor);
    }

    [Fact]
    public async Task HospitalDbContext_CanQueryMultipleDoctors()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var doctor1 = new Doctor { FirstName = "Doctor1", LastName = "Test1", Email = "d1@test.com", Password = "pass", IsActive = true };
        var doctor2 = new Doctor { FirstName = "Doctor2", LastName = "Test2", Email = "d2@test.com", Password = "pass", IsActive = true };
        var doctor3 = new Doctor { FirstName = "Doctor3", LastName = "Test3", Email = "d3@test.com", Password = "pass", IsActive = true };

        context.Doctors.AddRange(doctor1, doctor2, doctor3);
        await context.SaveChangesAsync();

        // Act
        var doctors = await context.Doctors.ToListAsync();

        // Assert
        Assert.Equal(3, doctors.Count);
    }

    [Fact]
    public async Task HospitalDbContext_CanQueryMultiplePatients()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var patient1 = new Patient { FirstName = "Patient1", LastName = "Test1", Email = "p1@test.com", Password = "pass", IsActive = true };
        var patient2 = new Patient { FirstName = "Patient2", LastName = "Test2", Email = "p2@test.com", Password = "pass", IsActive = true };

        context.Patients.AddRange(patient1, patient2);
        await context.SaveChangesAsync();

        // Act
        var patients = await context.Patients.ToListAsync();

        // Assert
        Assert.Equal(2, patients.Count);
    }

    [Fact]
    public async Task HospitalDbContext_SupportsConcurrentOperations()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var doctor = new Doctor { FirstName = "Concurrent", LastName = "Test", Email = "concurrent@test.com", Password = "pass", IsActive = true };
        var patient = new Patient { FirstName = "Concurrent", LastName = "Test", Email = "concurrent@test.com", Password = "pass", IsActive = true };

        // Act
        context.Doctors.Add(doctor);
        context.Patients.Add(patient);
        await context.SaveChangesAsync();

        // Assert
        Assert.True(doctor.Id > 0);
        Assert.True(patient.Id > 0);
    }

    [Fact]
    public async Task HospitalDbContext_CanFilterActiveDoctors()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var activeDoctor = new Doctor { FirstName = "Active", LastName = "Doctor", Email = "active@test.com", Password = "pass", IsActive = true };
        var inactiveDoctor = new Doctor { FirstName = "Inactive", LastName = "Doctor", Email = "inactive@test.com", Password = "pass", IsActive = false };

        context.Doctors.AddRange(activeDoctor, inactiveDoctor);
        await context.SaveChangesAsync();

        // Act
        var activeDoctors = await context.Doctors.Where(d => d.IsActive).ToListAsync();

        // Assert
        Assert.Single(activeDoctors);
        Assert.Equal("Active", activeDoctors[0].FirstName);
    }

    [Fact]
    public async Task HospitalDbContext_CanHandleEmptyDatabase()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);

        // Act
        var doctors = await context.Doctors.ToListAsync();
        var patients = await context.Patients.ToListAsync();
        var appointments = await context.Appointments.ToListAsync();

        // Assert
        Assert.Empty(doctors);
        Assert.Empty(patients);
        Assert.Empty(appointments);
    }

    [Fact]
    public void HospitalDbContext_Model_IsNotNull()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);

        // Act
        var model = context.Model;

        // Assert
        Assert.NotNull(model);
    }
}
