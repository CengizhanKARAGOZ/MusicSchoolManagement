using AutoMapper;
using MusicSchoolManagement.Core.DTOs.Payments;
using MusicSchoolManagement.Core.Entities;
using MusicSchoolManagement.Core.Enums;
using MusicSchoolManagement.Core.Exceptions;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Core.Interfaces.Services;

namespace MusicSchoolManagement.Business.Services;

public class PaymentService : IPaymentService
{
    #region Fields

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    #endregion

    #region Constructor

    public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #endregion

    #region Public Methods

    public async Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync()
    {
        var payments = await _unitOfWork.Payments.GetAllAsync();
        return _mapper.Map<IEnumerable<PaymentDto>>(payments);
    }

    public async Task<IEnumerable<PaymentDto>> GetPaymentsByStudentAsync(int studentId)
    {
        var payments = await _unitOfWork.Payments.GetByStudentIdAsync(studentId);
        return _mapper.Map<IEnumerable<PaymentDto>>(payments);
    }

    public async Task<IEnumerable<PaymentDto>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var payments = await _unitOfWork.Payments.GetByDateRangeAsync(startDate, endDate);
        return _mapper.Map<IEnumerable<PaymentDto>>(payments);
    }

    public async Task<IEnumerable<PaymentDto>> GetPendingPaymentsAsync()
    {
        var payments = await _unitOfWork.Payments.GetPendingPaymentsAsync();
        return _mapper.Map<IEnumerable<PaymentDto>>(payments);
    }

    public async Task<PaymentDto?> GetPaymentByIdAsync(int id)
    {
        var payment = await _unitOfWork.Payments.GetByIdAsync(id);
        if (payment == null)
            throw new NotFoundException("Payment", id);

        return _mapper.Map<PaymentDto>(payment);
    }

    public async Task<PaymentDto> CreatePaymentAsync(CreatePaymentDto createDto, int createdByUserId)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(createDto.StudentId);
        if (student == null)
            throw new NotFoundException("Student", createDto.StudentId);

        if (createDto.StudentPackageId.HasValue)
        {
            var studentPackage = await _unitOfWork.StudentPackages.GetByIdAsync(createDto.StudentPackageId.Value);
            if (studentPackage == null)
                throw new NotFoundException("Student Package", createDto.StudentPackageId.Value);
        }

        var payment = _mapper.Map<Payment>(createDto);
        payment.CreatedBy = createdByUserId;

        await _unitOfWork.Payments.AddAsync(payment);
        await _unitOfWork.SaveChangesAsync();

        // Reload with navigation properties
        payment = await _unitOfWork.Payments.GetByIdAsync(payment.Id);
        return _mapper.Map<PaymentDto>(payment!);
    }

    public async Task<decimal> GetTotalRevenueAsync(DateTime startDate, DateTime endDate)
    {
        return await _unitOfWork.Payments.GetTotalRevenueAsync(startDate, endDate);
    }

    public async Task<bool> RefundPaymentAsync(int id)
    {
        var payment = await _unitOfWork.Payments.GetByIdAsync(id);
        if (payment == null)
            throw new NotFoundException("Payment", id);

        if (payment.Status != PaymentStatus.Completed)
            throw new BadRequestException("Only completed payments can be refunded");

        payment.Status = PaymentStatus.Refunded;
        _unitOfWork.Payments.Update(payment);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    #endregion
}