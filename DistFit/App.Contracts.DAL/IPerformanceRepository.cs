using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IPerformanceRepository : IEntityRepository<App.DAL.DTO.Performance>, IOwnedEntityRepository<App.DAL.DTO.Performance>
{
    Task<IEnumerable<Performance>> GetAllByTypeIdAsync(Guid typeId, Guid userId, bool noTracking);
}