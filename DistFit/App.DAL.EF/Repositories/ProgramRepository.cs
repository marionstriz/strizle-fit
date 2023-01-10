using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ProgramRepository 
    : BaseEntityRepository<DAL.DTO.Program, Domain.Program, AppDbContext>, IProgramRepository
{
    public ProgramRepository(AppDbContext dbContext, IMapper<Program, Domain.Program> mapper) 
        : base(dbContext, mapper)
    {
    }
    
    public override async Task<DAL.DTO.Program?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(noTracking)
            .Include(u => u.DurationUnit)
            .FirstOrDefaultAsync(a => a.Id.Equals(id)));
    }

    public override async Task<IEnumerable<DAL.DTO.Program>> GetAllAsync(bool noTracking = true)
    {
        var query = CreateQuery(noTracking)
            .Include(u => u.DurationUnit);
        
        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }
}