using AutoMapper;
using MusicSchoolManagement.Core.DTOs.Common;
using MusicSchoolManagement.Core.Entities;
using MusicSchoolManagement.Core.Exceptions;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Core.Interfaces.Services;

namespace MusicSchoolManagement.Business.Services;

public class InstrumentService : IInstrumentService
{
    #region Fields

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    #endregion

    #region Constructor

    public InstrumentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #endregion

    #region Public Methods

    public async Task<IEnumerable<InstrumentDto>> GetAllInstrumentsAsync()
    {
        var instruments = await _unitOfWork.Instruments.GetAllAsync();
        return _mapper.Map<IEnumerable<InstrumentDto>>(instruments);
    }

    public async Task<IEnumerable<InstrumentDto>> GetActiveInstrumentsAsync()
    {
        var instruments = await _unitOfWork.Instruments.GetActiveInstrumentsAsync();
        return _mapper.Map<IEnumerable<InstrumentDto>>(instruments);
    }

    public async Task<InstrumentDto?> GetInstrumentByIdAsync(int id)
    {
        var instrument = await _unitOfWork.Instruments.GetByIdAsync(id);
        if (instrument == null)
            throw new NotFoundException("Instrument", id);

        return _mapper.Map<InstrumentDto>(instrument);
    }

    public async Task<InstrumentDto> CreateInstrumentAsync(string name, string? description)
    {
        var instrument = new Instrument
        {
            Name = name,
            Description = description,
            IsActive = true
        };

        await _unitOfWork.Instruments.AddAsync(instrument);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<InstrumentDto>(instrument);
    }

    public async Task<InstrumentDto?> UpdateInstrumentAsync(int id, string name, string? description, bool isActive)
    {
        var instrument = await _unitOfWork.Instruments.GetByIdAsync(id);
        if (instrument == null)
            throw new NotFoundException("Instrument", id);

        instrument.Name = name;
        instrument.Description = description;
        instrument.IsActive = isActive;

        _unitOfWork.Instruments.Update(instrument);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<InstrumentDto>(instrument);
    }

    public async Task<bool> DeleteInstrumentAsync(int id)
    {
        var instrument = await _unitOfWork.Instruments.GetByIdAsync(id);
        if (instrument == null)
            throw new NotFoundException("Instrument", id);

        _unitOfWork.Instruments.Remove(instrument);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    #endregion
}