using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for Appointment entity
/// </summary>
public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedOnAdd();

        builder.Property(a => a.PatientId).IsRequired();
        builder.Property(a => a.DoctorId).IsRequired();

        builder.Property(a => a.AppointmentDate).IsRequired();

        builder.Property(a => a.Symptoms).HasMaxLength(1000);
        builder.Property(a => a.Diagnosis).HasMaxLength(1000);

        builder.Property(a => a.Status).IsRequired();

        builder.Property(a => a.IsActive)
            .HasDefaultValue(true);

        builder.Property(a => a.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(a => a.CreatedBy)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.ModifiedBy)
            .HasMaxLength(200);

        // Relationships configured in Patient and Doctor configurations

        builder.HasMany(a => a.TreatmentHistories)
            .WithOne(t => t.Appointment)
            .HasForeignKey(t => t.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
