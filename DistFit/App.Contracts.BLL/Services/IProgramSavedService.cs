using App.BLL.DTO;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IProgramSavedService : IOwnedEntityService<App.BLL.DTO.ProgramSaved>
{
    Task<ProgramSaved?> GetProgramSaveByProgramAndUserIdAsync(Guid programId, Guid userId, bool noTracking = true);
}