using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for Staff entity
/// </summary>
public class StaffConfiguration : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder.ToTable("Staff");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedOnAdd();

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(s => s.Email).IsUnique();

        builder.Property(s => s.Password)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(s => s.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(s => s.Role)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.IsActive)
            .HasDefaultValue(true);

        builder.Property(s => s.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(s => s.CreatedBy)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.ModifiedBy)
            .HasMaxLength(200);
    }
}
