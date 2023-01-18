using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface ISetEntryRepository : IEntityRepository<App.DAL.DTO.SetEntry>, IOwnedEntityRepository<App.DAL.DTO.SetEntry>
{
    Task<IEnumerable<SetEntry>> GetAllByPerformanceIdAsync(Guid perfId, Guid userId, bool noTracking);
}