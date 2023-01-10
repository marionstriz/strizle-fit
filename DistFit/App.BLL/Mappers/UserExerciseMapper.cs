using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class UserExerciseMapper : BaseMapper<App.BLL.DTO.UserExercise, App.DAL.DTO.UserExercise>
{
    public UserExerciseMapper(IMapper mapper) : base(mapper)
    {
    }
}