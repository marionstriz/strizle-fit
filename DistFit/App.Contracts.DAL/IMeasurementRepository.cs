using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IMeasurementRepository : IOwnedEntityRepository<App.DAL.DTO.Measurement>, IEntityRepository<App.DAL.DTO.Measurement>
{
}