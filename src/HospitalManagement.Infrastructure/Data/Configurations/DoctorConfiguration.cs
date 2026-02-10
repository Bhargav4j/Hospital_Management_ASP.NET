using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for Doctor entity
/// </summary>
public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.ToTable("Doctors");

        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).ValueGeneratedOnAdd();

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(d => d.Email).IsUnique();

        builder.Property(d => d.Password)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(d => d.Specialization)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.Qualification)
            .HasMaxLength(200);

        builder.Property(d => d.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(d => d.ConsultationFee)
            .HasColumnType("decimal(18,2)");

        builder.Property(d => d.Address)
            .HasMaxLength(500);

        builder.Property(d => d.IsActive)
            .HasDefaultValue(true);

        builder.Property(d => d.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(d => d.CreatedBy)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.ModifiedBy)
            .HasMaxLength(200);

        // Relationships
        builder.HasMany(d => d.Appointments)
            .WithOne(a => a.Doctor)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.Bills)
            .WithOne(b => b.Doctor)
            .HasForeignKey(b => b.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
