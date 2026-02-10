using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for Patient entity
/// </summary>
public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("Patients");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(p => p.Email).IsUnique();

        builder.Property(p => p.Password)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.Gender)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(p => p.Address)
            .HasMaxLength(500);

        builder.Property(p => p.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(p => p.IsActive)
            .HasDefaultValue(true);

        builder.Property(p => p.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(p => p.CreatedBy)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.ModifiedBy)
            .HasMaxLength(200);

        // Relationships
        builder.HasMany(p => p.Appointments)
            .WithOne(a => a.Patient)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Bills)
            .WithOne(b => b.Patient)
            .HasForeignKey(b => b.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Feedbacks)
            .WithOne(f => f.Patient)
            .HasForeignKey(f => f.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Notifications)
            .WithOne(n => n.Patient)
            .HasForeignKey(n => n.PatientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
