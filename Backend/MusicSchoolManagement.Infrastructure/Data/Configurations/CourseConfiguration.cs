using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicSchoolManagement.Core.Entities;

namespace MusicSchoolManagement.Infrastructure.Data.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Courses");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(c => c.Level)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(c => c.Type)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(c => c.Duration)
            .IsRequired();

        builder.Property(c => c.BasePrice)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(c => c.MaxStudents)
            .HasDefaultValue(1);

        builder.Property(c => c.IsActive)
            .HasDefaultValue(true);

        builder.HasOne(c => c.Instrument)
            .WithMany(i => i.Courses)
            .HasForeignKey(c => c.InstrumentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}