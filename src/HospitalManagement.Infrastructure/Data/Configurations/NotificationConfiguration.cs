using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for Notification entity
/// </summary>
public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");

        builder.HasKey(n => n.Id);
        builder.Property(n => n.Id).ValueGeneratedOnAdd();

        builder.Property(n => n.PatientId).IsRequired();

        builder.Property(n => n.Message)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(n => n.IsRead)
            .HasDefaultValue(false);

        builder.Property(n => n.IsActive)
            .HasDefaultValue(true);

        builder.Property(n => n.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(n => n.CreatedBy)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(n => n.ModifiedBy)
            .HasMaxLength(200);
    }
}
