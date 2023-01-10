using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class UserSessionExerciseMapper : BaseMapper<UserSessionExercise, Domain.UserSessionExercise>
{
    public UserSessionExerciseMapper(IMapper mapper) : base(mapper)
    {
    }
}