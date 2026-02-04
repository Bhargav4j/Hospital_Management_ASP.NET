using ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for Feedback
/// </summary>
public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.ToTable("Feedbacks");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Message)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(f => f.Rating)
            .HasMaxLength(5);

        builder.Property(f => f.IsActive)
            .HasDefaultValue(true);

        builder.Property(f => f.CreatedDate)
            .HasDefaultValueSql("GETDATE()");

        builder.Property(f => f.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasDefaultValue("System");

        builder.Property(f => f.ModifiedBy)
            .HasMaxLength(100);

        builder.HasIndex(f => f.PatientId);
        builder.HasIndex(f => f.CreatedDate);
    }
}
