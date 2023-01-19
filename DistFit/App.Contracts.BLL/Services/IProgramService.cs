using App.BLL.DTO;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IProgramService : IEntityService<App.BLL.DTO.Program>
{
    Program AddProgramWithSave(Program program, Guid userId, bool noTracking = true);
}