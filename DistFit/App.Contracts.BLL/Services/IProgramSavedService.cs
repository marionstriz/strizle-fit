using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IProgramSavedService : 
    IOwnedEntityService<App.BLL.DTO.ProgramSaved>, IProgramSavedRepository<App.BLL.DTO.ProgramSaved>
{
    
}