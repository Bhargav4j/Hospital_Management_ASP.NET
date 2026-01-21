using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Data;

/// <summary>
/// Database context for Hospital Management System
/// </summary>
public class HospitalDbContext : DbContext
{
    public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options)
    {
    }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<TimeSlot> TimeSlots { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<Staff> Staff { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HospitalDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                if (entry.Entity.GetType().GetProperty("CreatedDate") != null)
                {
                    entry.Property("CreatedDate").CurrentValue = DateTime.UtcNow;
                }
                if (entry.Entity.GetType().GetProperty("IsActive") != null)
                {
                    entry.Property("IsActive").CurrentValue = true;
                }
            }

            if (entry.State == EntityState.Modified)
            {
                if (entry.Entity.GetType().GetProperty("ModifiedDate") != null)
                {
                    entry.Property("ModifiedDate").CurrentValue = DateTime.UtcNow;
                }
            }
        }
    }
}
