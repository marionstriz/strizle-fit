using System.Linq;
using App.Contracts.DAL;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Session = App.DAL.DTO.Session;

namespace App.DAL.EF.Repositories;

public class SessionRepository 
    : BaseEntityRepository<DAL.DTO.Session, Domain.Session, AppDbContext>, ISessionRepository
{
    public SessionRepository(AppDbContext dbContext, IMapper<Session, Domain.Session> mapper) 
        : base(dbContext, mapper)
    {
    }
    
    public override async Task<DAL.DTO.Session?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(noTracking)
            .Include(u => u.Program)
            .FirstOrDefaultAsync(a => a.Id.Equals(id)));
    }

    public override async Task<IEnumerable<DAL.DTO.Session>> GetAllAsync(bool noTracking = true)
    {
        var query = CreateQuery(noTracking)
            .Include(u => u.Program);

        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }
}