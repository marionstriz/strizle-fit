using App.Contracts.DAL;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Measurement = App.DAL.DTO.Measurement;

namespace App.DAL.EF.Repositories;

public class MeasurementRepository 
    : BaseEntityRepository<DAL.DTO.Measurement, Domain.Measurement, AppDbContext>, IMeasurementRepository
{
    public MeasurementRepository(AppDbContext dbContext, IMapper<Measurement, Domain.Measurement> mapper) 
        : base(dbContext, mapper)
    {
    }
    
    public override async Task<DAL.DTO.Measurement?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(noTracking)
            .Include(u => u.AppUser)
            .Include(u => u.ValueUnit)
            .Include(u => u.MeasurementType)
            .FirstOrDefaultAsync(a => a.Id.Equals(id)));
    }

    public async Task<IEnumerable<DAL.DTO.Measurement>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking)
            .Include(u => u.AppUser)
            .Include(u => u.MeasurementType)
            .Include(u => u.ValueUnit)
            .Where(m => m.AppUserId == userId);
        
        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }
}