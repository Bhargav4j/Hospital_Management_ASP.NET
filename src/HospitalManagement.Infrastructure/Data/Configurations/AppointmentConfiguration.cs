using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for Appointment entity
/// </summary>
public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointment");

        builder.HasKey(a => a.AppointmentID);

        builder.Property(a => a.Status)
            .HasConversion<int>();

        builder.Property(a => a.Disease)
            .HasMaxLength(200);

        builder.Property(a => a.Progress)
            .HasMaxLength(500);

        builder.Property(a => a.Prescription)
            .HasMaxLength(1000);

        builder.Property(a => a.BillAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(a => a.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.ModifiedBy)
            .HasMaxLength(100);

        builder.Property(a => a.IsActive)
            .HasDefaultValue(true);

        // Relationships
        builder.HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.TimeSlot)
            .WithMany(t => t.Appointments)
            .HasForeignKey(a => a.TimeSlotID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.Feedbacks)
            .WithOne(f => f.Appointment)
            .HasForeignKey(f => f.AppointmentID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
