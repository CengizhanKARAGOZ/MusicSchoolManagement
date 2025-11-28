using MusicSchoolManagement.Core.DTOs.Packages;
using MusicSchoolManagement.Core.Entities;
using MusicSchoolManagement.Core.Enums;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Core.Interfaces.Services;

namespace MusicSchoolManagement.Business.Services;

public class PackageService : IPackageService
{
    private readonly IUnitOfWork _unitOfWork;

    public PackageService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    #region Package Operations
    
    public async Task<IEnumerable<PackageDto>> GetAllPackagesAsync()
    {
        var packages = await _unitOfWork.Packages.GetAllAsync();
        return packages.Select(MapToDto);
    }

    public async Task<IEnumerable<PackageDto>> GetActivePackagesAsync()
    {
        var packages = await _unitOfWork.Packages.GetActivePackagesAsync();
        return packages.Select(MapToDto);
    }

    public async Task<PackageDto?> GetPackageByIdAsync(int id)
    {
        var package = await _unitOfWork.Packages.GetByIdAsync(id);
        return package == null ? null : MapToDto(package);
    }

    public async Task<PackageDto> CreatePackageAsync(CreatePackageDto createDto)
    {
        var totalLessons = createDto.DurationMonths * createDto.LessonsPerMonth;

        var package = new Package
        {
            Name = createDto.Name,
            DurationMonths = createDto.DurationMonths,
            LessonsPerMonth = createDto.LessonsPerMonth,
            TotalLessons = totalLessons,
            Price = createDto.Price,
            DiscountPercentage = createDto.DiscountPercentage,
            Description = createDto.Description,
            IsActive = true
        };

        await _unitOfWork.Packages.AddAsync(package);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(package);
    }

    public async Task<PackageDto?> UpdatePackageAsync(int id, UpdatePackageDto updateDto)
    {
        var package = await _unitOfWork.Packages.GetByIdAsync(id);
        if (package == null)
            return null;

        var totalLessons = updateDto.DurationMonths * updateDto.LessonsPerMonth;

        package.Name = updateDto.Name;
        package.DurationMonths = updateDto.DurationMonths;
        package.LessonsPerMonth = updateDto.LessonsPerMonth;
        package.TotalLessons = totalLessons;
        package.Price = updateDto.Price;
        package.DiscountPercentage = updateDto.DiscountPercentage;
        package.Description = updateDto.Description;
        package.IsActive = updateDto.IsActive;

        _unitOfWork.Packages.Update(package);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(package);
    }

    public async Task<bool> DeletePackageAsync(int id)
    {
        var package = await _unitOfWork.Packages.GetByIdAsync(id);
        if (package == null)
            return false;

        _unitOfWork.Packages.Remove(package);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    #endregion

    #region Student Package Operations

    public async Task<IEnumerable<StudentPackageDto>> GetStudentPackagesAsync(int studentId)
    {
        var studentPackages = await _unitOfWork.StudentPackages.GetByStudentIdAsync(studentId);
        return studentPackages.Select(MapToStudentPackageDto);
    }

    public async Task<StudentPackageDto?> GetStudentPackageByIdAsync(int id)
    {
        var studentPackage = await _unitOfWork.StudentPackages.GetByIdAsync(id);
        if (studentPackage == null)
            return null;

        // Load related entities
        var student = await _unitOfWork.Students.GetByIdAsync(studentPackage.StudentId);
        var package = await _unitOfWork.Packages.GetByIdAsync(studentPackage.PackageId);
        var course = await _unitOfWork.Courses.GetByIdAsync(studentPackage.CourseId);

        if (student == null || package == null || course == null)
            return null;

        return new StudentPackageDto
        {
            Id = studentPackage.Id,
            StudentId = studentPackage.StudentId,
            StudentName = $"{student.FirstName} {student.LastName}",
            PackageId = studentPackage.PackageId,
            PackageName = package.Name,
            CourseId = studentPackage.CourseId,
            CourseName = course.Name,
            StartDate = studentPackage.StartDate,
            EndDate = studentPackage.EndDate,
            TotalLessons = studentPackage.TotalLessons,
            UsedLessons = studentPackage.UsedLessons,
            RemainingLessons = studentPackage.RemainingLessons,
            Status = studentPackage.Status,
            PurchaseDate = studentPackage.PurchaseDate
        };
    }

    public async Task<StudentPackageDto> AssignPackageToStudentAsync(AssignPackageDto assignDto)
    {
        // Validate student exists
        var student = await _unitOfWork.Students.GetByIdAsync(assignDto.StudentId);
        if (student == null)
            throw new InvalidOperationException("Student not found");

        // Validate package exists
        var package = await _unitOfWork.Packages.GetByIdAsync(assignDto.PackageId);
        if (package == null)
            throw new InvalidOperationException("Package not found");

        // Validate course exists
        var course = await _unitOfWork.Courses.GetByIdAsync(assignDto.CourseId);
        if (course == null)
            throw new InvalidOperationException("Course not found");

        // Check if student already has an active package for this course
        var existingPackage = await _unitOfWork.StudentPackages.GetActivePackageAsync(assignDto.StudentId, assignDto.CourseId);
        if (existingPackage != null)
            throw new InvalidOperationException("Student already has an active package for this course");

        var endDate = assignDto.StartDate.AddMonths(package.DurationMonths);

        var studentPackage = new StudentPackage
        {
            StudentId = assignDto.StudentId,
            PackageId = assignDto.PackageId,
            CourseId = assignDto.CourseId,
            StartDate = assignDto.StartDate,
            EndDate = endDate,
            TotalLessons = package.TotalLessons,
            UsedLessons = 0,
            RemainingLessons = package.TotalLessons,
            Status = StudentPackageStatus.Active,
            PurchaseDate = DateTime.UtcNow
        };

        await _unitOfWork.StudentPackages.AddAsync(studentPackage);
        await _unitOfWork.SaveChangesAsync();

        return new StudentPackageDto
        {
            Id = studentPackage.Id,
            StudentId = studentPackage.StudentId,
            StudentName = $"{student.FirstName} {student.LastName}",
            PackageId = studentPackage.PackageId,
            PackageName = package.Name,
            CourseId = studentPackage.CourseId,
            CourseName = course.Name,
            StartDate = studentPackage.StartDate,
            EndDate = studentPackage.EndDate,
            TotalLessons = studentPackage.TotalLessons,
            UsedLessons = studentPackage.UsedLessons,
            RemainingLessons = studentPackage.RemainingLessons,
            Status = studentPackage.Status,
            PurchaseDate = studentPackage.PurchaseDate
        };
    }

    public async Task<bool> CancelStudentPackageAsync(int id)
    {
        var studentPackage = await _unitOfWork.StudentPackages.GetByIdAsync(id);
        if (studentPackage == null)
            return false;

        studentPackage.Status = StudentPackageStatus.Cancelled;
        _unitOfWork.StudentPackages.Update(studentPackage);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    #endregion

    private static PackageDto MapToDto(Package package)
    {
        return new PackageDto
        {
            Id = package.Id,
            Name = package.Name,
            DurationMonths = package.DurationMonths,
            LessonsPerMonth = package.LessonsPerMonth,
            TotalLessons = package.TotalLessons,
            Price = package.Price,
            DiscountPercentage = package.DiscountPercentage,
            Description = package.Description,
            IsActive = package.IsActive
        };
    }

    private static StudentPackageDto MapToStudentPackageDto(StudentPackage sp)
    {
        return new StudentPackageDto
        {
            Id = sp.Id,
            StudentId = sp.StudentId,
            StudentName = $"{sp.Student.FirstName} {sp.Student.LastName}",
            PackageId = sp.PackageId,
            PackageName = sp.Package.Name,
            CourseId = sp.CourseId,
            CourseName = sp.Course.Name,
            StartDate = sp.StartDate,
            EndDate = sp.EndDate,
            TotalLessons = sp.TotalLessons,
            UsedLessons = sp.UsedLessons,
            RemainingLessons = sp.RemainingLessons,
            Status = sp.Status,
            PurchaseDate = sp.PurchaseDate
        };
    }
}