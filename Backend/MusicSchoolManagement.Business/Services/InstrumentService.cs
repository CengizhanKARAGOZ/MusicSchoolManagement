using MusicSchoolManagement.Core.DTOs.Common;
using MusicSchoolManagement.Core.Entities;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Core.Interfaces.Services;

namespace MusicSchoolManagement.Business.Services;

public class InstrumentService : IInstrumentService
{
    private readonly IUnitOfWork _unitOfWork;

    public InstrumentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<InstrumentDto>> GetAllInstrumentsAsync()
    {
        var instruments = await _unitOfWork.Instruments.GetAllAsync();
        return instruments.Select(MapToDto);
    }

    public async Task<IEnumerable<InstrumentDto>> GetActiveInstrumentsAsync()
    {
        var instruments = await _unitOfWork.Instruments.GetActiveInstrumentsAsync();
        return instruments.Select(MapToDto);
    }

    public async Task<InstrumentDto?> GetInstrumentByIdAsync(int id)
    {
        var instrument =  await _unitOfWork.Instruments.GetByIdAsync(id);
        return instrument == null ? null : MapToDto(instrument);
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

        return MapToDto(instrument);
    }

    public async Task<InstrumentDto?> UpdateInstrumentAsync(int id, string name, string? description, bool isActive)
    {
        var instrument = await _unitOfWork.Instruments.GetByIdAsync(id);
        if (instrument == null)
            return null;

        instrument.Name = name;
        instrument.Description = description;
        instrument.IsActive = isActive;

        _unitOfWork.Instruments.Update(instrument);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(instrument);
    }

    public async Task<bool> DeleteInstrumentAsync(int id)
    {
        var instrument = await _unitOfWork.Instruments.GetByIdAsync(id);
        if (instrument == null)
            return false;
        
        _unitOfWork.Instruments.Remove(instrument);
        await _unitOfWork.SaveChangesAsync();
        
        return true;
    }
    
    private static InstrumentDto MapToDto(Instrument instrument)
    {
        return new InstrumentDto
        {
            Id = instrument.Id,
            Name = instrument.Name,
            Description = instrument.Description,
            IsActive = instrument.IsActive
        };
    }
}