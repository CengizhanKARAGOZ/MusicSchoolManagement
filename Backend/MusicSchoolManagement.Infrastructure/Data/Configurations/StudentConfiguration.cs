using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicSchoolManagement.Core.Enitties;

namespace MusicSchoolManagement.Infrastructure.Data.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Email)
            .HasMaxLength(150);

        builder.Property(s => s.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(s => s.ParentName)
            .HasMaxLength(100);

        builder.Property(s => s.ParentPhone)
            .HasMaxLength(20);

        builder.Property(s => s.ParentEmail)
            .HasMaxLength(150);

        builder.Property(s => s.EmergencyContact)
            .HasMaxLength(20);

        builder.Property(s => s.RegistrationDate)
            .IsRequired();

        builder.Property(s => s.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(s => s.Email);
        builder.HasIndex(s => s.PhoneNumber);
    }
}