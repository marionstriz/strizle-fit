using App.BLL.DTO;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IPerformanceService : IOwnedEntityService<App.BLL.DTO.Performance>
{
    Task<IEnumerable<Performance>> GetAllByTypeIdAsync(Guid typeId, Guid userId, bool noTracking = true);
}