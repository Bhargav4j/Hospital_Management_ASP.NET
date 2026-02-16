using ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Infrastructure.Data.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointment");
        
        builder.HasKey(a => a.AppointmentID);
        
        builder.Property(a => a.AppointmentDate)
            .IsRequired();
        
        builder.Property(a => a.Timings)
            .HasMaxLength(50);
        
        builder.Property(a => a.Status)
            .HasMaxLength(50)
            .HasDefaultValue("Pending");
        
        builder.Property(a => a.Disease)
            .HasMaxLength(200);
        
        builder.Property(a => a.Progress)
            .HasMaxLength(1000);
        
        builder.Property(a => a.Prescription)
            .HasMaxLength(2000);
        
        builder.Property(a => a.BillAmount)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0);
        
        builder.Property(a => a.IsPaid)
            .HasDefaultValue(false);
        
        builder.Property(a => a.FeedbackGiven)
            .HasDefaultValue(false);
        
        builder.Property(a => a.CreatedDate)
            .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
        
        builder.HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientID)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
