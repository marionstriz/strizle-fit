using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class GoalMapper : BaseMapper<App.BLL.DTO.Goal, App.DAL.DTO.Goal>
{
    public GoalMapper(IMapper mapper) : base(mapper)
    {
    }
}