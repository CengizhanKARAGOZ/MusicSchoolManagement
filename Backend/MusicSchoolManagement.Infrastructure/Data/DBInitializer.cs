using MusicSchoolManagement.Core.Enums;
using Microsoft.EntityFrameworkCore;
using MusicSchoolManagement.Core.Enitties;
using MusicSchoolManagement.Core.Entities;
using MusicSchoolManagement.Core.Helpers;

namespace MusicSchoolManagement.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Check if admin exists
        if (await context.Users.AnyAsync())  // context.Users.Any() yerine await context.Users.AnyAsync()
            return;

        // Create default admin
        var admin = new User
        {
            FirstName = "Admin",
            LastName = "User",
            Email = "admin@musicschool.com",
            PhoneNumber = "+905551234567",
            PasswordHash = PasswordHelper.HashPassword("Admin123!"),
            Role = UserRole.Admin,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        context.Users.Add(admin);
        await context.SaveChangesAsync();
    }
}