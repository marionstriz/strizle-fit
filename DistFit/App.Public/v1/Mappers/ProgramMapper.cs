using App.Public.DTO.v1;
using AutoMapper;
using Base.DAL;
using Base.Domain;

namespace App.Public.v1.Mappers;

public class ProgramMapper : BaseMapper<App.Public.DTO.v1.Program, App.BLL.DTO.Program>
{
    public ProgramMapper(IMapper mapper) : base(mapper)
    {
    }
    
    public override BLL.DTO.Program? Map(Program? entity)
    {
        return Map(entity, LangStr.SupportedCultureOrDefault(
            Thread.CurrentThread.CurrentUICulture.Name));
    }

    public BLL.DTO.Program? Map(Program? entity, string culture)
    {
        if (entity == null)
        {
            return null;
        }

        var name = new LangStr(entity.Name, culture);
        LangStr? description = null;
        if (entity.Description != null)
        {
            description = new LangStr(entity.Description, culture);
        }

        return new BLL.DTO.Program
        {
            Id = entity.Id,
            Name = name,
            Description = description,
            IsPublic = entity.IsPublic,
            Duration = entity.Duration,
            DurationUnit = Mapper.Map<Unit?, BLL.DTO.Unit?>(entity.DurationUnit),
            DurationUnitId = entity.DurationUnitId,
        };
    }
}