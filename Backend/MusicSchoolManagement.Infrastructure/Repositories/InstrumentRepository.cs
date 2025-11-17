using Microsoft.EntityFrameworkCore;
using MusicSchoolManagement.Core.Entities;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Infrastructure.Data;

namespace MusicSchoolManagement.Infrastructure.Repositories;

public class InstrumentRepository : GenericRepository<Instrument>, IInstrumentRepository
{
    public InstrumentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Instrument?> GetByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(i => i.Name == name);
    }

    public async Task<IEnumerable<Instrument>> GetActiveInstrumentsAsync()
    {
        return await _dbSet.Where(i => i.IsActive).ToListAsync();
    }
}