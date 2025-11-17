using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicSchoolManagement.Core.Entities;

namespace MusicSchoolManagement.Infrastructure.Data.Configurations;

public class TeacherAvailabilityConfiguration : IEntityTypeConfiguration<TeacherAvailability>
{
    public void Configure(EntityTypeBuilder<TeacherAvailability> builder)
    {
        builder.ToTable("TeacherAvailabilities");

        builder.HasKey(ta => ta.Id);

        builder.Property(ta => ta.DayOfWeek)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(ta => ta.StartTime)
            .IsRequired();

        builder.Property(ta => ta.EndTime)
            .IsRequired();

        builder.Property(ta => ta.IsAvailable)
            .HasDefaultValue(true);

        builder.HasOne(ta => ta.Teacher)
            .WithMany(t => t.Availabilities)
            .HasForeignKey(ta => ta.TeacherId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ta => new { ta.TeacherId, ta.DayOfWeek });
    }
}