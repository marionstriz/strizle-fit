using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class PerformanceService 
    : BaseEntityService<App.BLL.DTO.Performance, App.DAL.DTO.Performance, IPerformanceRepository>, IPerformanceService
{
    public PerformanceService(
        IPerformanceRepository repository, 
        IMapper<Performance, DAL.DTO.Performance> mapper
    ) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<Performance>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(userId, noTracking)).Select(x => Mapper.Map(x)!);
    }
    
    public async Task<IEnumerable<Performance>> GetAllByTypeIdAsync(Guid typeId, Guid userId, bool noTracking = true)
    {
        return (await Repository.GetAllByTypeIdAsync(typeId, userId, noTracking)).Select(x => Mapper.Map(x)!);
    }
}