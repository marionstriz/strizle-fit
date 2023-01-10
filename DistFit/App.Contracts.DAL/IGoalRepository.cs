using Base.Contracts.DAL;
using Base.Contracts.Domain;

namespace App.Contracts.DAL;

public interface IGoalRepository : IOwnedEntityRepository<App.DAL.DTO.Goal>, IEntityRepository<App.DAL.DTO.Goal>
{
}