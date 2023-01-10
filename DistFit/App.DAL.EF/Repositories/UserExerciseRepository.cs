using App.Contracts.DAL;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using UserExercise = App.DAL.DTO.UserExercise;

namespace App.DAL.EF.Repositories;

public class UserExerciseRepository 
    : BaseEntityRepository<UserExercise, Domain.UserExercise, AppDbContext>, IUserExerciseRepository
{
    public UserExerciseRepository(
        AppDbContext dbContext, 
        IMapper<UserExercise, Domain.UserExercise> mapper) 
        : base(dbContext, mapper)
    {
    }
    
    public override async Task<UserExercise?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(noTracking)
            .Include(u => u.AppUser).Include(u => u.ExerciseType)
            .FirstOrDefaultAsync(a => a.Id.Equals(id)));
    }

    public async Task<IEnumerable<UserExercise>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking)
            .Include(u => u.AppUser).Include(u => u.ExerciseType)
            .Where(m => m.AppUserId == userId);
        
        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }
}