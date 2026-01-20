using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for Doctor entity
/// </summary>
public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    /// <summary>
    /// Configures the Doctor entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.ToTable("Doctor");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(d => d.Email)
            .IsUnique();

        builder.Property(d => d.Password)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(d => d.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(d => d.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(d => d.Specialization)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Qualification)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.ConsultationFee)
            .HasPrecision(18, 2);

        builder.Property(d => d.IsActive)
            .HasDefaultValue(true);

        builder.Property(d => d.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(d => d.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.ModifiedBy)
            .HasMaxLength(100);

        builder.HasMany(d => d.Appointments)
            .WithOne(a => a.Doctor)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.TreatmentHistories)
            .WithOne(t => t.Doctor)
            .HasForeignKey(t => t.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.Bills)
            .WithOne(b => b.Doctor)
            .HasForeignKey(b => b.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.Feedbacks)
            .WithOne(f => f.Doctor)
            .HasForeignKey(f => f.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
