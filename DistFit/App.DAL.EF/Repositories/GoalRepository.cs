using App.Contracts.DAL;
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
}