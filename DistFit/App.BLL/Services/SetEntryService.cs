using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class SetEntryService 
    : BaseEntityService<App.BLL.DTO.SetEntry, App.DAL.DTO.SetEntry, ISetEntryRepository>, ISetEntryService
{
    public SetEntryService(
        ISetEntryRepository repository, 
        IMapper<SetEntry, DAL.DTO.SetEntry> mapper
    ) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<SetEntry>> GetAllAsync(Guid userId, bool noTracking)
    {
        return (await Repository.GetAllAsync(userId, noTracking)).Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<SetEntry>> GetAllByPerformanceIdAsync(Guid perfId, Guid userId, bool noTracking = true)
    {
        return (await Repository.GetAllByPerformanceIdAsync(perfId, userId, noTracking)).Select(x => Mapper.Map(x)!);
    }
}