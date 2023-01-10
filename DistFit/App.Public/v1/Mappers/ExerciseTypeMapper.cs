using App.Public.DTO.v1;
using AutoMapper;
using Base.DAL;
using Base.Domain;

namespace App.Public.v1.Mappers;

public class ExerciseTypeMapper : BaseMapper<App.Public.DTO.v1.ExerciseType, App.BLL.DTO.ExerciseType>
{
    public ExerciseTypeMapper(IMapper mapper) : base(mapper)
    {
    }
    
    public override BLL.DTO.ExerciseType? Map(ExerciseType? entity)
    {
        return Map(entity, LangStr.SupportedCultureOrDefault(
            Thread.CurrentThread.CurrentUICulture.Name));
    }

    public BLL.DTO.ExerciseType? Map(ExerciseType? entity, string culture)
    {
        if (entity == null)
        {
            return null;
        }

        LangStr name = new LangStr(entity.Name, culture);

        return new BLL.DTO.ExerciseType
        {
            Id = entity.Id,
            Name = name
        };
    }
}