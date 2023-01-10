using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class GoalMapper : BaseMapper<Goal, Domain.Goal>
{
    public GoalMapper(IMapper mapper) : base(mapper)
    {
    }
}