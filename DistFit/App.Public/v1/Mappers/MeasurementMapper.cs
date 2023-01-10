using AutoMapper;
using Base.DAL;

namespace App.Public.v1.Mappers;

public class MeasurementMapper : BaseMapper<App.Public.DTO.v1.Measurement, App.BLL.DTO.Measurement>
{
    public MeasurementMapper(IMapper mapper) : base(mapper)
    {
    }
}