using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class ExerciseTypeMapper : BaseMapper<App.BLL.DTO.ExerciseType, App.DAL.DTO.ExerciseType>
{
    public ExerciseTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}