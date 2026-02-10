using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for TreatmentHistory entity
/// </summary>
public class TreatmentHistoryConfiguration : IEntityTypeConfiguration<TreatmentHistory>
{
    public void Configure(EntityTypeBuilder<TreatmentHistory> builder)
    {
        builder.ToTable("TreatmentHistories");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedOnAdd();

        builder.Property(t => t.AppointmentId).IsRequired();

        builder.Property(t => t.Prescription).HasMaxLength(2000);
        builder.Property(t => t.Notes).HasMaxLength(2000);

        builder.Property(t => t.TreatmentDate).IsRequired();

        builder.Property(t => t.IsActive)
            .HasDefaultValue(true);

        builder.Property(t => t.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(t => t.CreatedBy)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.ModifiedBy)
            .HasMaxLength(200);
    }
}
