using ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for Staff
/// </summary>
public class StaffConfiguration : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder.ToTable("Staff");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(s => s.Email)
            .IsUnique();

        builder.Property(s => s.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(s => s.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(s => s.Role)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Staff");

        builder.Property(s => s.IsActive)
            .HasDefaultValue(true);

        builder.Property(s => s.CreatedDate)
            .HasDefaultValueSql("GETDATE()");

        builder.Property(s => s.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasDefaultValue("System");

        builder.Property(s => s.ModifiedBy)
            .HasMaxLength(100);
    }
}
