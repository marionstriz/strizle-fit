using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class UserExerciseMapper : BaseMapper<UserExercise, Domain.UserExercise>
{
    public UserExerciseMapper(IMapper mapper) : base(mapper)
    {
    }
}