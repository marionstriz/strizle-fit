using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class UserSessionExerciseMapper : BaseMapper<App.BLL.DTO.UserSessionExercise, App.DAL.DTO.UserSessionExercise>
{
    public UserSessionExerciseMapper(IMapper mapper) : base(mapper)
    {
    }
}