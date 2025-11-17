using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicSchoolManagement.Core.Entities;

namespace MusicSchoolManagement.Infrastructure.Data.Configurations;

public class AttendanceLogConfiguration : IEntityTypeConfiguration<AttendanceLog>
{
    public void Configure(EntityTypeBuilder<AttendanceLog> builder)
    {
        builder.ToTable("AttendanceLogs");

        builder.HasKey(al => al.Id);

        builder.Property(al => al.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(al => al.RecordedBy)
            .IsRequired();

        builder.Property(al => al.RecordedAt)
            .IsRequired();

        builder.HasOne(al => al.Appointment)
            .WithMany(a => a.AttendanceLogs)
            .HasForeignKey(al => al.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(al => al.Student)
            .WithMany(s => s.AttendanceLogs)
            .HasForeignKey(al => al.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(al => al.RecordedByUser)
            .WithMany()
            .HasForeignKey(al => al.RecordedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(al => al.AppointmentId);
        builder.HasIndex(al => al.StudentId);
    }
}