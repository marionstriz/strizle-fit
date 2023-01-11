using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IMeasurementRepository : 
    IOwnedEntityRepository<App.DAL.DTO.Measurement>, IEntityRepository<App.DAL.DTO.Measurement>
{
    Task<IEnumerable<Measurement>> GetAllByTypeIdAsync(Guid typeId, Guid userId, bool noTracking);
}