using ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Infrastructure.Data.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("Patient");
        
        builder.HasKey(p => p.PatientID);
        
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(p => p.Email)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.HasIndex(p => p.Email)
            .IsUnique();
        
        builder.Property(p => p.Password)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(p => p.Phone)
            .HasMaxLength(20);
        
        builder.Property(p => p.Address)
            .HasMaxLength(500);
        
        builder.Property(p => p.Gender)
            .HasMaxLength(10);
        
        builder.Property(p => p.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");
    }
}
