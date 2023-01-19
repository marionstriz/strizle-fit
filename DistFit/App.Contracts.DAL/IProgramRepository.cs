using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IProgramRepository : IEntityRepository<App.DAL.DTO.Program>
{
    Program AddProgramWithSave(Program program, Guid userId, bool noTracking = true);
}