using System.Security.Claims;
using Base.Contracts.DAL;
using Base.Contracts.Domain;

namespace App.Contracts.DAL;

public interface IProgramSavedRepository : IProgramSavedRepository<App.DAL.DTO.ProgramSaved>
{
}

public interface IProgramSavedRepository<TEntity> : IOwnedEntityRepository<TEntity>, IEntityRepository<TEntity> 
    where TEntity : class, IDomainEntityId
{
    Task<IEnumerable<TEntity>> GetAllByUserAndGroupsAsync(ClaimsPrincipal user, bool noTracking);
}