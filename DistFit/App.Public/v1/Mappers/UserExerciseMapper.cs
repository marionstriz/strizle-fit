using AutoMapper;
using Base.DAL;

namespace App.Public.v1.Mappers;

public class UserExerciseMapper : BaseMapper<App.Public.DTO.v1.UserExercise, App.BLL.DTO.UserExercise>
{
    public UserExerciseMapper(IMapper mapper) : base(mapper)
    {
    }
}