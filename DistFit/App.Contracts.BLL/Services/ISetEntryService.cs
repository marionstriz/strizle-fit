using App.BLL.DTO;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface ISetEntryService : IOwnedEntityService<App.BLL.DTO.SetEntry>
{
    Task<IEnumerable<SetEntry>> GetAllByPerformanceIdAsync(Guid perfId, Guid userId, bool noTracking = true);
}