using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicSchoolManagement.Core.Entities;

namespace MusicSchoolManagement.Infrastructure.Data.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.AppointmentDate)
            .IsRequired();

        builder.Property(a => a.StartTime)
            .IsRequired();

        builder.Property(a => a.EndTime)
            .IsRequired();

        builder.Property(a => a.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(a => a.IsRecurring)
            .HasDefaultValue(false);

        builder.Property(a => a.RecurringPattern)
            .HasMaxLength(50);

        builder.Property(a => a.CreatedBy)
            .IsRequired();

        builder.HasOne(a => a.Student)
            .WithMany(s => s.Appointments)
            .HasForeignKey(a => a.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Teacher)
            .WithMany(t => t.Appointments)
            .HasForeignKey(a => a.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Course)
            .WithMany(c => c.Appointments)
            .HasForeignKey(a => a.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Classroom)
            .WithMany(c => c.Appointments)
            .HasForeignKey(a => a.ClassroomId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(a => a.StudentPackage)
            .WithMany(sp => sp.Appointments)
            .HasForeignKey(a => a.StudentPackageId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(a => a.ParentAppointment)
            .WithMany(a => a.ChildAppointments)
            .HasForeignKey(a => a.ParentAppointmentId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(a => a.CreatedByUser)
            .WithMany()
            .HasForeignKey(a => a.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes for performance
        builder.HasIndex(a => a.AppointmentDate);
        builder.HasIndex(a => new { a.TeacherId, a.AppointmentDate });
        builder.HasIndex(a => new { a.StudentId, a.AppointmentDate });
        builder.HasIndex(a => new { a.ClassroomId, a.AppointmentDate });
    }
}