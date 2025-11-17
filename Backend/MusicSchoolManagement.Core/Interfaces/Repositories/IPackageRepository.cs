using MusicSchoolManagement.Core.Entities;

namespace MusicSchoolManagement.Core.Interfaces.Repositories;

public interface IPackageRepository : IGenericRepository<Package>
{
    Task<IEnumerable<Package>> GetActivePackagesAsync();
}