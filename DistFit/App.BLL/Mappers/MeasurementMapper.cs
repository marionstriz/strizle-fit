using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class MeasurementMapper : BaseMapper<App.BLL.DTO.Measurement, App.DAL.DTO.Measurement>
{
    public MeasurementMapper(IMapper mapper) : base(mapper)
    {
    }
}