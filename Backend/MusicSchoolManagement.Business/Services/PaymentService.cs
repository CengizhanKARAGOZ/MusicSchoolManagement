using MusicSchoolManagement.Core.DTOs.Payments;
using MusicSchoolManagement.Core.Enitties;
using MusicSchoolManagement.Core.Entities;
using MusicSchoolManagement.Core.Enums;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Core.Interfaces.Services;

namespace MusicSchoolManagement.Business.Services;

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _unitOfWork;

    public PaymentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync()
    {
        var payments = await _unitOfWork.Payments.GetAllAsync();
        var paymentDtos = new List<PaymentDto>();

        foreach (var payment in payments)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(payment.StudentId);
            var packageName = payment.StudentPackageId.HasValue
                ? (await _unitOfWork.StudentPackages.GetByIdAsync(payment.StudentPackageId.Value))?.Package.Name
                : null;
            
            paymentDtos.Add(MapToDto(payment, student, packageName));
        }

        return paymentDtos;
    }

    public async Task<IEnumerable<PaymentDto>> GetPaymentsByStudentAsync(int studentId)
    {
        var payments = await _unitOfWork.Payments.GetByStudentIdAsync(studentId);
        return payments.Select(p => MapToDto(p, p.Student, p.StudentPackage?.Package?.Name));
    }

    public async Task<IEnumerable<PaymentDto>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var payments = await _unitOfWork.Payments.GetByDateRangeAsync(startDate, endDate);
        return payments.Select(p => MapToDto(
            p, 
            p.Student!, 
            p.StudentPackage?.Package?.Name
        ));
    }

    public async Task<IEnumerable<PaymentDto>> GetPendingPaymentsAsync()
    {
        var payments = await _unitOfWork.Payments.GetPendingPaymentsAsync();
        return payments.Select(p => MapToDto(
            p, 
            p.Student!, 
            p.StudentPackage?.Package?.Name
        ));
    }

    public async Task<PaymentDto?> GetPaymentByIdAsync(int id)
    {
        var payment =  await _unitOfWork.Payments.GetByIdAsync(id);
        if (payment == null)
            return null;
        
        var student = await _unitOfWork.Students.GetByIdAsync(payment.StudentId);
        var packageName = payment.StudentPackageId.HasValue
            ? (await _unitOfWork.StudentPackages.GetByIdAsync(payment.StudentPackageId.Value))?.Package.Name
            : null;
        
        return MapToDto(payment, student, packageName);
    }

    public async Task<PaymentDto> CreatePaymentAsync(CreatePaymentDto createDto, int createdByUserId)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(createDto.StudentId);
        if (student == null)
            throw new InvalidOperationException("Student not found");

        string? packageName = null;
        if (createDto.StudentPackageId.HasValue)
        {
            var studentPackage = await _unitOfWork.StudentPackages.GetByIdAsync(createDto.StudentPackageId.Value);
            if (studentPackage == null)
                throw new InvalidOperationException("Student package not found");
        
            packageName = studentPackage.Package.Name;
        }

        var payment = new Payment
        {
            StudentId = createDto.StudentId,
            StudentPackageId = createDto.StudentPackageId,
            Amount = createDto.Amount,
            PaymentDate = createDto.PaymentDate,
            PaymentMethod = createDto.PaymentMethod,
            Status = createDto.Status,
            TransactionReference = createDto.TransactionReference,
            Notes = createDto.Notes,
            CreatedBy = createdByUserId
        };

        await _unitOfWork.Payments.AddAsync(payment);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(payment, student, packageName);
    }

    public async Task<decimal> GetTotalRevenueAsync(DateTime startDate, DateTime endDate)
    {
        return await _unitOfWork.Payments.GetTotalRevenueAsync(startDate, endDate);
    }

    public async Task<bool> RefundPaymentAsync(int id)
    {
        var payment = await _unitOfWork.Payments.GetByIdAsync(id);
        if (payment == null)
            return false;
        
        if(payment.Status != PaymentStatus.Completed)
            throw new InvalidOperationException("Only completed payments can be refunded.");
        
        payment.Status = PaymentStatus.Refunded;
        _unitOfWork.Payments.Update(payment);
        await _unitOfWork.SaveChangesAsync();
        
        return true;
    }
    
    private static PaymentDto MapToDto(Payment payment, Student? student, string? packageName)
    {
        return new PaymentDto
        {
            Id = payment.Id,
            StudentId = payment.StudentId,
            StudentName = student != null ? $"{student.FirstName} {student.LastName}" : "Unknown",
            StudentPackageId = payment.StudentPackageId,
            PackageName = packageName ?? payment.StudentPackage?.Package?.Name,
            Amount = payment.Amount,
            PaymentDate = payment.PaymentDate,
            PaymentMethod = payment.PaymentMethod,
            Status = payment.Status,
            TransactionReference = payment.TransactionReference,
            Notes = payment.Notes,
            CreatedAt = payment.CreatedAt
        };
    }
}