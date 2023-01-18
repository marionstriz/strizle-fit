using App.Contracts.DAL;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Performance = App.DAL.DTO.Performance;

namespace App.DAL.EF.Repositories;

public class PerformanceRepository 
    : BaseEntityRepository<DAL.DTO.Performance, Domain.Performance, AppDbContext>, IPerformanceRepository
{
    public PerformanceRepository(AppDbContext dbContext, IMapper<Performance, Domain.Performance> mapper) 
        : base(dbContext, mapper)
    {
    }
    
    public override async Task<DAL.DTO.Performance?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(noTracking)
            .Include(u => u.UserExercise)
            .ThenInclude(e => e!.ExerciseType)
            .FirstOrDefaultAsync(a => a.Id.Equals(id)));
    }

    public async Task<IEnumerable<DAL.DTO.Performance>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking)
            .Include(u => u.UserExercise)
            .ThenInclude(e => e!.ExerciseType)
            .Where(u => u.UserExercise!.AppUserId == userId);
        
        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }
    
    public async Task<IEnumerable<Performance>> GetAllByTypeIdAsync(Guid typeId, Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking)
            .Include(u => u.UserExercise)
            .ThenInclude(e => e!.ExerciseType)
            .Where(u => u.UserExercise!.AppUserId == userId 
                        && u.UserExercise.ExerciseTypeId == typeId);
        
        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }
}