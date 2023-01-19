using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class ProgramService 
    : BaseEntityService<App.BLL.DTO.Program, App.DAL.DTO.Program, IProgramRepository>, IProgramService
{
    public ProgramService(
        IProgramRepository repository, 
        IMapper<Program, DAL.DTO.Program> mapper
    ) : base(repository, mapper)
    {
    }

    public Program AddProgramWithSave(Program program, Guid userId, bool noTracking = true)
    {
        return Mapper.Map(Repository.AddProgramWithSave(Mapper.Map(program)!, userId))!;
    }
}