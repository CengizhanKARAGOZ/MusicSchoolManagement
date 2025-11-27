using MusicSchoolManagement.Core.DTOs.Common;

namespace MusicSchoolManagement.Core.Interfaces.Services;

public interface IInstrumentService
{
    Task<IEnumerable<InstrumentDto>> GetAllInstrumentsAsync();
    Task<IEnumerable<InstrumentDto>> GetActiveInstrumentsAsync();
    Task<InstrumentDto?> GetInstrumentByIdAsync(int id);
    Task<InstrumentDto> CreateInstrumentAsync(string name, string? description);
    Task<InstrumentDto?> UpdateInstrumentAsync(int id, string name, string? description, bool isActive);
    Task<bool> DeleteInstrumentAsync(int id);
}