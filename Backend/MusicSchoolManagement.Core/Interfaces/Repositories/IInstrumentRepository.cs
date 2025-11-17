using MusicSchoolManagement.Core.Entities;

namespace MusicSchoolManagement.Core.Interfaces.Repositories;

public interface IInstrumentRepository : IGenericRepository<Instrument>
{
    Task<Instrument?> GetByNameAsync(string name);
    Task<IEnumerable<Instrument>> GetActiveInstrumentsAsync();
}