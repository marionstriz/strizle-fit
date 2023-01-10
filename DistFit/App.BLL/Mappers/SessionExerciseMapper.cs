using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class SessionExerciseMapper : BaseMapper<App.BLL.DTO.SessionExercise, App.DAL.DTO.SessionExercise>
{
    public SessionExerciseMapper(IMapper mapper) : base(mapper)
    {
    }
}