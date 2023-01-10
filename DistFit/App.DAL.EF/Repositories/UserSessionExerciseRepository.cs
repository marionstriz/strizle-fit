using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class UserSessionExerciseRepository 
    : BaseEntityRepository<UserSessionExercise, Domain.UserSessionExercise, AppDbContext>, IUserSessionExerciseRepository
{
    public UserSessionExerciseRepository(
        AppDbContext dbContext, 
        IMapper<UserSessionExercise, Domain.UserSessionExercise> mapper) 
        : base(dbContext, mapper)
    {
    }
    
    public override async Task<UserSessionExercise?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(noTracking)
            .Include(u => u.UserExercise)
            .Include(u => u.SessionExercise)
            .FirstOrDefaultAsync(a => a.Id.Equals(id)));
    }

    public override async Task<IEnumerable<UserSessionExercise>> GetAllAsync(bool noTracking = true)
    {
        var query = CreateQuery(noTracking)
            .Include(u => u.UserExercise)
            .Include(u => u.SessionExercise);
        
        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }
}