using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for Feedback entity
/// </summary>
public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    /// <summary>
    /// Configures the Feedback entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.ToTable("Feedback");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.FeedbackText)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(f => f.Rating)
            .IsRequired();

        builder.Property(f => f.IsActive)
            .HasDefaultValue(true);

        builder.Property(f => f.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(f => f.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.ModifiedBy)
            .HasMaxLength(100);

        builder.HasIndex(f => f.FeedbackDate);
        builder.HasIndex(f => f.Rating);
    }
}
