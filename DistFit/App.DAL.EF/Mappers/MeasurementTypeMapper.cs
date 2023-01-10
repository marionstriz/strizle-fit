using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class MeasurementTypeMapper : BaseMapper<MeasurementType, Domain.MeasurementType>
{
    public MeasurementTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}