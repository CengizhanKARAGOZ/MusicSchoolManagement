using MusicSchoolManagement.Core.Enitties;
using MusicSchoolManagement.Core.Enums;

namespace MusicSchoolManagement.Core.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public bool IsActive { get; set; } = true;
    public bool PasswordChangeRequired { get; set; } = false;
    
    //Navigation Properties
    public Teacher? Teacher { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}