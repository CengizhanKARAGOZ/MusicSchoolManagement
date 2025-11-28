using MusicSchoolManagement.Core.DTOs.Packages;

namespace MusicSchoolManagement.Core.Interfaces.Services;

public interface IPackageService
{
    Task<IEnumerable<PackageDto>> GetAllPackagesAsync();
    Task<IEnumerable<PackageDto>> GetActivePackagesAsync();
    Task<PackageDto?> GetPackageByIdAsync(int id);
    Task<PackageDto> CreatePackageAsync(CreatePackageDto createDto);
    Task<PackageDto?> UpdatePackageAsync(int id, UpdatePackageDto updateDto);
    Task<bool> DeletePackageAsync(int id);
    
    // Student Package Operations
    Task<IEnumerable<StudentPackageDto>> GetStudentPackagesAsync(int studentId);
    Task<StudentPackageDto?> GetStudentPackageByIdAsync(int id);
    Task<StudentPackageDto> AssignPackageToStudentAsync(AssignPackageDto assignDto);
    Task<bool> CancelStudentPackageAsync(int id);
}