using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class ExerciseTypeMapper : BaseMapper<ExerciseType, Domain.ExerciseType>
{
    public ExerciseTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}