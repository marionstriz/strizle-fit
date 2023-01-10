using System.Security.Claims;
using App.Contracts.DAL;
using Base.Contracts.Base;
using Base.DAL.EF;
using Base.Extensions;
using Microsoft.EntityFrameworkCore;
using ProgramSaved = App.DAL.DTO.ProgramSaved;

namespace App.DAL.EF.Repositories;

public class ProgramSavedRepository 
    : BaseEntityRepository<DAL.DTO.ProgramSaved, Domain.ProgramSaved, AppDbContext>, IProgramSavedRepository
{
    public ProgramSavedRepository(AppDbContext dbContext, IMapper<ProgramSaved, Domain.ProgramSaved> mapper) 
        : base(dbContext, mapper)
    {
    }
    
    public override async Task<DAL.DTO.ProgramSaved?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(noTracking)
            .Include(u => u.AppUser)
            .Include(u => u.Program)
            .FirstOrDefaultAsync(a => a.Id.Equals(id)));
    }

    public async Task<IEnumerable<DAL.DTO.ProgramSaved>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking)
            .Include(u => u.AppUser)
            .Include(u => u.Program)
            .Where(m => m.AppUserId == userId);
        
        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<ProgramSaved>> GetAllByUserAndGroupsAsync(
        ClaimsPrincipal claimsPrincipal, bool noTracking = true)
    {
        var query = CreateQuery(noTracking)
            .Include(u => u.AppUser)
            .Include(u => u.Program)
            .Where(m => 
                m.AppUserId == claimsPrincipal.GetUserId());
        
        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }
}