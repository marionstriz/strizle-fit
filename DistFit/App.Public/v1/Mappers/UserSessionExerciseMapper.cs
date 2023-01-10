using AutoMapper;
using Base.DAL;

namespace App.Public.v1.Mappers;

public class UserSessionExerciseMapper : BaseMapper<App.Public.DTO.v1.UserSessionExercise, App.BLL.DTO.UserSessionExercise>
{
    public UserSessionExerciseMapper(IMapper mapper) : base(mapper)
    {
    }
}