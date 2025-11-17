using Microsoft.EntityFrameworkCore;
using MusicSchoolManagement.Core.Entities;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Infrastructure.Data;

namespace MusicSchoolManagement.Infrastructure.Repositories;

public class PackageRepository : GenericRepository<Package>, IPackageRepository
{
    public PackageRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Package>> GetActivePackagesAsync()
    {
        return await _dbSet.Where(p => p.IsActive).ToListAsync();
    }
}