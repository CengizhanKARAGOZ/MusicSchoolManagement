using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicSchoolManagement.Core.Entities;

namespace MusicSchoolManagement.Infrastructure.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Amount)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(p => p.PaymentDate)
            .IsRequired();

        builder.Property(p => p.PaymentMethod)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(p => p.TransactionReference)
            .HasMaxLength(100);

        builder.Property(p => p.CreatedBy)
            .IsRequired();

        builder.HasOne(p => p.Student)
            .WithMany(s => s.Payments)
            .HasForeignKey(p => p.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.StudentPackage)
            .WithMany(sp => sp.Payments)
            .HasForeignKey(p => p.StudentPackageId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.CreatedByUser)
            .WithMany()
            .HasForeignKey(p => p.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => p.PaymentDate);
        builder.HasIndex(p => p.StudentId);
    }
}