using AutoMapper;
using Base.DAL;

namespace App.Public.v1.Mappers;

public class SessionExerciseMapper : BaseMapper<App.Public.DTO.v1.SessionExercise, App.BLL.DTO.SessionExercise>
{
    public SessionExerciseMapper(IMapper mapper) : base(mapper)
    {
    }
}