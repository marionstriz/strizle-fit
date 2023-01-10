using App.Public.DTO.v1;
using AutoMapper;
using Base.DAL;
using Base.Domain;

namespace App.Public.v1.Mappers;

public class UnitMapper : BaseMapper<App.Public.DTO.v1.Unit, App.BLL.DTO.Unit>
{
    public UnitMapper(IMapper mapper) : base(mapper)
    {
    }
    
    public override BLL.DTO.Unit? Map(Unit? entity)
    {
        return Map(entity, LangStr.SupportedCultureOrDefault(
            Thread.CurrentThread.CurrentUICulture.Name));
    }

    public BLL.DTO.Unit? Map(Unit? entity, string culture)
    {
        if (entity == null)
        {
            return null;
        }

        var name = new LangStr(entity.Name, culture);
        var symbol = new LangStr(entity.Symbol, culture);
        
        return new BLL.DTO.Unit
        {
            Id = entity.Id,
            Name = name,
            Symbol = symbol
        };
    }
}