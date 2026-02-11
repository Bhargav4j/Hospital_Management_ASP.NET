using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Data.Configurations;

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
            .IsRequired();

        builder.Property(f => f.IsActive)
            .HasDefaultValue(true);

        builder.Property(f => f.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(f => f.CreatedBy)
            .HasMaxLength(100)
            .HasDefaultValue("System");

        builder.Property(f => f.ModifiedBy)
            .HasMaxLength(100);

        builder.HasOne(f => f.Patient)
            .WithMany(u => u.Feedbacks)
            .HasForeignKey(f => f.PatientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
