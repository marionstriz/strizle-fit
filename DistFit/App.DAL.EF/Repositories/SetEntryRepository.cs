using App.Contracts.DAL;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using SetEntry = App.DAL.DTO.SetEntry;

namespace App.DAL.EF.Repositories;

public class SetEntryRepository 
    : BaseEntityRepository<DAL.DTO.SetEntry, Domain.SetEntry, AppDbContext>, ISetEntryRepository
{
    public SetEntryRepository(AppDbContext dbContext, IMapper<SetEntry, Domain.SetEntry> mapper) 
        : base(dbContext, mapper)
    {
    }
    
    public override async Task<DAL.DTO.SetEntry?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(noTracking)
            .Include(u => u.QuantityUnit)
            .Include(u => u.WeightUnit)
            .Include(u => u.Performance)
            .FirstOrDefaultAsync(a => a.Id.Equals(id)));
    }

    public override async Task<IEnumerable<DAL.DTO.SetEntry>> GetAllAsync(bool noTracking = true)
    {
        var query = CreateQuery(noTracking)
            .Include(u => u.QuantityUnit)
            .Include(u => u.WeightUnit)
            .Include(u => u.Performance);

        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }
}