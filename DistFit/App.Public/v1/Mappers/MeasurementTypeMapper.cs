using App.Public.DTO.v1;
using AutoMapper;
using Base.DAL;
using Base.Domain;

namespace App.Public.v1.Mappers;

public class MeasurementTypeMapper : BaseMapper<App.Public.DTO.v1.MeasurementType, App.BLL.DTO.MeasurementType>
{
    public MeasurementTypeMapper(IMapper mapper) : base(mapper)
    {
    }
    
    public override BLL.DTO.MeasurementType? Map(MeasurementType? entity)
    {
        return Map(entity, LangStr.SupportedCultureOrDefault(
            Thread.CurrentThread.CurrentUICulture.Name));
    }

    public BLL.DTO.MeasurementType? Map(MeasurementType? entity, string culture)
    {
        if (entity == null)
        {
            return null;
        }

        LangStr name = new LangStr(entity.Name, culture);

        return new BLL.DTO.MeasurementType
        {
            Id = entity.Id,
            Name = name
        };
    }
}