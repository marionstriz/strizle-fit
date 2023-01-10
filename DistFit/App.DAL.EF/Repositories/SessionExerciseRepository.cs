using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class SessionExerciseRepository 
    : BaseEntityRepository<DAL.DTO.SessionExercise, Domain.SessionExercise, AppDbContext>, ISessionExerciseRepository
{
    public SessionExerciseRepository(AppDbContext dbContext, IMapper<SessionExercise, Domain.SessionExercise> mapper) 
        : base(dbContext, mapper)
    {
    }
    
    public override async Task<DAL.DTO.SessionExercise?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(noTracking)
            .Include(u => u.Session)
            .Include(u => u.ExerciseType)
            .FirstOrDefaultAsync(a => a.Id.Equals(id)));
    }

    public override async Task<IEnumerable<DAL.DTO.SessionExercise>> GetAllAsync(bool noTracking = true)
    {
        var query = CreateQuery(noTracking)
            .Include(u => u.Session)
            .Include(u => u.ExerciseType);

        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }
}