using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class GoalService 
    : BaseEntityService<App.BLL.DTO.Goal, App.DAL.DTO.Goal, IGoalRepository>, IGoalService
{
    public GoalService(
        IGoalRepository repository, 
        IMapper<Goal, DAL.DTO.Goal> mapper
    ) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<Goal>> GetAllAsync(Guid userId, bool noTracking)
    {
        return (await Repository.GetAllAsync(userId, noTracking)).Select(x => Mapper.Map(x)!);
    }
}