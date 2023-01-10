using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class MeasurementTypeService 
    : BaseEntityService<App.BLL.DTO.MeasurementType, App.DAL.DTO.MeasurementType, IMeasurementTypeRepository>, IMeasurementTypeService
{
    public MeasurementTypeService(
        IMeasurementTypeRepository repository, 
        IMapper<MeasurementType, DAL.DTO.MeasurementType> mapper
    ) : base(repository, mapper)
    {
    }
}