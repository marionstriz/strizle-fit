using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class MeasurementService 
    : BaseEntityService<App.BLL.DTO.Measurement, App.DAL.DTO.Measurement, IMeasurementRepository>, IMeasurementService
{
    public MeasurementService(
        IMeasurementRepository repository, 
        IMapper<Measurement, DAL.DTO.Measurement> mapper
    ) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<Measurement>> GetAllAsync(Guid userId, bool noTracking)
    {
        return (await Repository.GetAllAsync(userId, noTracking)).Select(x => Mapper.Map(x)!);
    }
}