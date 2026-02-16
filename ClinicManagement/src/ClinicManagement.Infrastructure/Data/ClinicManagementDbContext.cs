using ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace ClinicManagement.Infrastructure.Data;

public class ClinicManagementDbContext : DbContext
{
    public ClinicManagementDbContext(DbContextOptions<ClinicManagementDbContext> options)
        : base(options)
    {
        // Enable legacy timestamp behavior for PostgreSQL
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<OtherStaff> OtherStaff => Set<OtherStaff>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Set default schema to public
        modelBuilder.HasDefaultSchema("public");

        // Apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClinicManagementDbContext).Assembly);

        // Configure PostgreSQL extensions if needed
        modelBuilder.HasPostgresExtension("uuid-ossp");
    }
}
