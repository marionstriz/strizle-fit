using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class GoalRepository : BaseEntityRepository<DAL.DTO.Goal, Domain.Goal, AppDbContext>, IGoalRepository
{
    public GoalRepository(
        AppDbContext dbContext, 
        IMapper<DAL.DTO.Goal, Domain.Goal> mapper
    ) : base(dbContext, mapper)
    {
    }
    
    public override async Task<DAL.DTO.Goal?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(noTracking)
            .Include(u => u.AppUser)
            .Include(u => u.ExerciseType)
            .Include(u => u.MeasurementType)
            .Include(u => u.ValueUnit)
            .FirstOrDefaultAsync(a => a.Id.Equals(id)));
    }

    public async Task<IEnumerable<DAL.DTO.Goal>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking)
            .Include(u => u.AppUser)
            .Include(u => u.ExerciseType)
            .Include(u => u.MeasurementType)
            .Include(u => u.ValueUnit)
            .Where(m => m.AppUserId == userId);
        
        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }

    public override Goal Update(Goal entity)
    {
        var domainGoal = Mapper.Map(entity)!;
        domainGoal.UpdatedAt = DateTime.UtcNow;
        domainGoal.UpdatedBy = domainGoal.AppUser?.UserName;
        
        return Mapper.Map(RepoDbSet.Update(domainGoal).Entity)!;
    }

    public override Goal Add(Goal entity)
    {
        var domainGoal = Mapper.Map(entity)!;

        domainGoal.CreatedBy = domainGoal.AppUser?.UserName;
        domainGoal.UpdatedBy = domainGoal.AppUser?.UserName;
        
        return Mapper.Map(RepoDbSet.Add(domainGoal).Entity)!;
    }
}