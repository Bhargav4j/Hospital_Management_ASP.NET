using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for Feedback entity
/// </summary>
public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.ToTable("Feedbacks");

        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).ValueGeneratedOnAdd();

        builder.Property(f => f.PatientId).IsRequired();

        builder.Property(f => f.Message)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(f => f.IsActive)
            .HasDefaultValue(true);

        builder.Property(f => f.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(f => f.CreatedBy)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(f => f.ModifiedBy)
            .HasMaxLength(200);
    }
}
