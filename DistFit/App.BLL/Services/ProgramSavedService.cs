using System.Security.Claims;
using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class ProgramSavedService 
    : BaseEntityService<App.BLL.DTO.ProgramSaved, App.DAL.DTO.ProgramSaved, IProgramSavedRepository>, IProgramSavedService
{
    public ProgramSavedService(
        IProgramSavedRepository repository, 
        IMapper<ProgramSaved, DAL.DTO.ProgramSaved> mapper
    ) : base(repository, mapper)
    {
    }
    
    public async Task<IEnumerable<ProgramSaved>> GetAllAsync(Guid userId, bool noTracking)
    {
        return (await Repository.GetAllAsync(userId, noTracking)).Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<ProgramSaved>> GetAllByUserAndGroupsAsync(ClaimsPrincipal user, bool noTracking)
    {
        return (await Repository.GetAllByUserAndGroupsAsync(user, noTracking)).Select(x => Mapper.Map(x)!);
    }
}