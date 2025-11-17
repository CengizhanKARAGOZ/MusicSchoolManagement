using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicSchoolManagement.Core.Entities;

namespace MusicSchoolManagement.Infrastructure.Data.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Type)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(n => n.Subject)
            .HasMaxLength(255);

        builder.Property(n => n.Message)
            .IsRequired();

        builder.Property(n => n.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.HasOne(n => n.User)
            .WithMany()
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(n => n.Student)
            .WithMany()
            .HasForeignKey(n => n.StudentId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(n => n.Status);
        builder.HasIndex(n => n.CreatedAt);
    }
}