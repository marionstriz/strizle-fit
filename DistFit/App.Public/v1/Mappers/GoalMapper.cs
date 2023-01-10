using AutoMapper;
using Base.DAL;

namespace App.Public.v1.Mappers;

public class GoalMapper : BaseMapper<App.Public.DTO.v1.Goal, App.BLL.DTO.Goal>
{
    public GoalMapper(IMapper mapper) : base(mapper)
    {
    }
}