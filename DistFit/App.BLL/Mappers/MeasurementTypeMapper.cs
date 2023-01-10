using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class MeasurementTypeMapper : BaseMapper<App.BLL.DTO.MeasurementType, App.DAL.DTO.MeasurementType>
{
    public MeasurementTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}