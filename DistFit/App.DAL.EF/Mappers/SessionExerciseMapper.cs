using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class SessionExerciseMapper : BaseMapper<SessionExercise, Domain.SessionExercise>
{
    public SessionExerciseMapper(IMapper mapper) : base(mapper)
    {
    }
}