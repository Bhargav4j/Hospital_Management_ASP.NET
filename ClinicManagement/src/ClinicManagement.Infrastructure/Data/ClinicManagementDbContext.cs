using ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Infrastructure.Data;

public class ClinicManagementDbContext : DbContext
{
    public ClinicManagementDbContext(DbContextOptions<ClinicManagementDbContext> options)
        : base(options)
    {
    }

    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<OtherStaff> OtherStaff => Set<OtherStaff>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClinicManagementDbContext).Assembly);
    }
}
