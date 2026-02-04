using Xunit;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Infrastructure.Data.Tests;

public class ClinicDbContextTests
{
    [Fact]
    public void ClinicDbContext_Constructor_ShouldCreateInstance()
    {
        var options = new DbContextOptionsBuilder<ClinicDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        var context = new ClinicDbContext(options);

        Assert.NotNull(context);
    }

    [Fact]
    public void ClinicDbContext_Patients_ShouldReturnDbSet()
    {
        var options = new DbContextOptionsBuilder<ClinicDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb2")
            .Options;

        using var context = new ClinicDbContext(options);

        Assert.NotNull(context.Patients);
        Assert.IsAssignableFrom<DbSet<Patient>>(context.Patients);
    }

    [Fact]
    public void ClinicDbContext_Doctors_ShouldReturnDbSet()
    {
        var options = new DbContextOptionsBuilder<ClinicDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb3")
            .Options;

        using var context = new ClinicDbContext(options);

        Assert.NotNull(context.Doctors);
        Assert.IsAssignableFrom<DbSet<Doctor>>(context.Doctors);
    }

    [Fact]
    public void ClinicDbContext_Appointments_ShouldReturnDbSet()
    {
        var options = new DbContextOptionsBuilder<ClinicDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb4")
            .Options;

        using var context = new ClinicDbContext(options);

        Assert.NotNull(context.Appointments);
        Assert.IsAssignableFrom<DbSet<Appointment>>(context.Appointments);
    }

    [Fact]
    public void ClinicDbContext_Clinics_ShouldReturnDbSet()
    {
        var options = new DbContextOptionsBuilder<ClinicDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb5")
            .Options;

        using var context = new ClinicDbContext(options);

        Assert.NotNull(context.Clinics);
        Assert.IsAssignableFrom<DbSet<Clinic>>(context.Clinics);
    }

    [Fact]
    public void ClinicDbContext_Bills_ShouldReturnDbSet()
    {
        var options = new DbContextOptionsBuilder<ClinicDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb6")
            .Options;

        using var context = new ClinicDbContext(options);

        Assert.NotNull(context.Bills);
        Assert.IsAssignableFrom<DbSet<Bill>>(context.Bills);
    }

    [Fact]
    public void ClinicDbContext_Feedbacks_ShouldReturnDbSet()
    {
        var options = new DbContextOptionsBuilder<ClinicDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb7")
            .Options;

        using var context = new ClinicDbContext(options);

        Assert.NotNull(context.Feedbacks);
        Assert.IsAssignableFrom<DbSet<Feedback>>(context.Feedbacks);
    }

    [Fact]
    public void ClinicDbContext_Staff_ShouldReturnDbSet()
    {
        var options = new DbContextOptionsBuilder<ClinicDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb8")
            .Options;

        using var context = new ClinicDbContext(options);

        Assert.NotNull(context.Staff);
        Assert.IsAssignableFrom<DbSet<Staff>>(context.Staff);
    }
}
