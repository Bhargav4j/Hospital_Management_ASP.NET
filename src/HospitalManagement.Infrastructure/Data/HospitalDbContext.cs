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

    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Bill> Bills => Set<Bill>();
    public DbSet<TreatmentHistory> TreatmentHistories => Set<TreatmentHistory>();
    public DbSet<Feedback> Feedbacks => Set<Feedback>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Staff> Staff => Set<Staff>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HospitalDbContext).Assembly);
    }
}
