using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class MeasurementMapper : BaseMapper<Measurement, Domain.Measurement>
{
    public MeasurementMapper(IMapper mapper) : base(mapper)
    {
    }
}