using AutoMapper;
using MusicSchoolManagement.Core.DTOs.Packages;
using MusicSchoolManagement.Core.Entities;
using MusicSchoolManagement.Core.Enums;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Core.Interfaces.Services;

namespace MusicSchoolManagement.Business.Services;

public class PackageService : IPackageService
{
    #region Fields

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    #endregion

    #region Constructor

    public PackageService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #endregion

    #region Package Operations

    public async Task<IEnumerable<PackageDto>> GetAllPackagesAsync()
    {
        var packages = await _unitOfWork.Packages.GetAllAsync();
        return _mapper.Map<IEnumerable<PackageDto>>(packages);
    }

    public async Task<IEnumerable<PackageDto>> GetActivePackagesAsync()
    {
        var packages = await _unitOfWork.Packages.GetActivePackagesAsync();
        return _mapper.Map<IEnumerable<PackageDto>>(packages);
    }

    public async Task<PackageDto?> GetPackageByIdAsync(int id)
    {
        var package = await _unitOfWork.Packages.GetByIdAsync(id);
        return package == null ? null : _mapper.Map<PackageDto>(package);
    }

    public async Task<PackageDto> CreatePackageAsync(CreatePackageDto createDto)
    {
        var package = _mapper.Map<Package>(createDto);

        await _unitOfWork.Packages.AddAsync(package);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<PackageDto>(package);
    }

    public async Task<PackageDto?> UpdatePackageAsync(int id, UpdatePackageDto updateDto)
    {
        var package = await _unitOfWork.Packages.GetByIdAsync(id);
        if (package == null)
            return null;

        _mapper.Map(updateDto, package);

        _unitOfWork.Packages.Update(package);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<PackageDto>(package);
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
        return _mapper.Map<IEnumerable<StudentPackageDto>>(studentPackages);
    }

    public async Task<StudentPackageDto?> GetStudentPackageByIdAsync(int id)
    {
        var studentPackage = await _unitOfWork.StudentPackages.GetByIdAsync(id);
        return studentPackage == null ? null : _mapper.Map<StudentPackageDto>(studentPackage);
    }

    public async Task<StudentPackageDto> AssignPackageToStudentAsync(AssignPackageDto assignDto)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(assignDto.StudentId);
        if (student == null)
            throw new InvalidOperationException("Student not found");

        var package = await _unitOfWork.Packages.GetByIdAsync(assignDto.PackageId);
        if (package == null)
            throw new InvalidOperationException("Package not found");

        var course = await _unitOfWork.Courses.GetByIdAsync(assignDto.CourseId);
        if (course == null)
            throw new InvalidOperationException("Course not found");

        var existingPackage = await _unitOfWork.StudentPackages.GetActivePackageAsync(assignDto.StudentId, assignDto.CourseId);
        if (existingPackage != null)
            throw new InvalidOperationException("Student already has an active package for this course");

        var studentPackage = _mapper.Map<StudentPackage>(assignDto);
        studentPackage.EndDate = assignDto.StartDate.AddMonths(package.DurationMonths);
        studentPackage.TotalLessons = package.TotalLessons;
        studentPackage.RemainingLessons = package.TotalLessons;

        await _unitOfWork.StudentPackages.AddAsync(studentPackage);
        await _unitOfWork.SaveChangesAsync();

        studentPackage = await _unitOfWork.StudentPackages.GetByIdAsync(studentPackage.Id);
        return _mapper.Map<StudentPackageDto>(studentPackage!);
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
}