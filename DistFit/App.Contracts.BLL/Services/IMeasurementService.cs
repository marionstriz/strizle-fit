using App.BLL.DTO;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IMeasurementService : IOwnedEntityService<App.BLL.DTO.Measurement>
{
    Task<IEnumerable<Measurement>> GetAllByTypeIdAsync(Guid typeId, Guid userId, bool noTracking);
}