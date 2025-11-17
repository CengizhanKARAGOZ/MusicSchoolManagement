using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicSchoolManagement.Core.Entities;

namespace MusicSchoolManagement.Infrastructure.Data.Configurations;

public class StudentPackageConfiguration : IEntityTypeConfiguration<StudentPackage>
{
    public void Configure(EntityTypeBuilder<StudentPackage> builder)
    {
        builder.ToTable("StudentPackages");

        builder.HasKey(sp => sp.Id);

        builder.Property(sp => sp.StartDate)
            .IsRequired();

        builder.Property(sp => sp.EndDate)
            .IsRequired();

        builder.Property(sp => sp.TotalLessons)
            .IsRequired();

        builder.Property(sp => sp.UsedLessons)
            .HasDefaultValue(0);

        builder.Property(sp => sp.RemainingLessons)
            .IsRequired();

        builder.Property(sp => sp.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.HasOne(sp => sp.Student)
            .WithMany(s => s.StudentPackages)
            .HasForeignKey(sp => sp.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(sp => sp.Package)
            .WithMany(p => p.StudentPackages)
            .HasForeignKey(sp => sp.PackageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(sp => sp.Course)
            .WithMany(c => c.StudentPackages)
            .HasForeignKey(sp => sp.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(sp => new { sp.StudentId, sp.CourseId, sp.Status });
    }
}