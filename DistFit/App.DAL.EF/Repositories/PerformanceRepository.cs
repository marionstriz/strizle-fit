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
            .Include(u => u.Exercise)
            .FirstOrDefaultAsync(a => a.Id.Equals(id)));
    }

    public override async Task<IEnumerable<DAL.DTO.Performance>> GetAllAsync(bool noTracking = true)
    {
        var query = CreateQuery(noTracking)
            .Include(u => u.Exercise);
        
        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }
}