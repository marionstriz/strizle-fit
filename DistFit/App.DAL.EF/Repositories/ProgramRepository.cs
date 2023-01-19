using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ProgramRepository 
    : BaseEntityRepository<DAL.DTO.Program, Domain.Program, AppDbContext>, IProgramRepository
{
    
    private readonly IMapper<ProgramSaved, Domain.ProgramSaved> PsMapper;
    public ProgramRepository(AppDbContext dbContext, IMapper<Program, Domain.Program> mapper, IMapper<ProgramSaved, Domain.ProgramSaved> psMapper) 
        : base(dbContext, mapper)
    {
        PsMapper = psMapper;
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
            .Include(u => u.DurationUnit)
            .Where(u => u.IsPublic);
        
        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }

    public Program AddProgramWithSave(Program program, Guid userId, bool noTracking = true)
    {
        if (program.Id == Guid.Empty) program.Id = new Guid();
        
        var programSave = new ProgramSaved
        {
            AppUserId = userId,
            IsCreator = true,
            ProgramId = program.Id
        };
        RepoDbContext.ProgramsSaved.Add(PsMapper.Map(programSave)!);
        return Mapper.Map(RepoDbSet.Add(Mapper.Map(program)!).Entity)!;
    }
}