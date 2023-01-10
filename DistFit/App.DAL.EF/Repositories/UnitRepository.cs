using App.Contracts.DAL;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Unit = App.DAL.DTO.Unit;

namespace App.DAL.EF.Repositories;

public class UnitRepository : BaseEntityRepository<DAL.DTO.Unit, Domain.Unit, AppDbContext>, IUnitRepository
{
    public UnitRepository(AppDbContext dbContext, IMapper<Unit, Domain.Unit> mapper) 
        : base(dbContext, mapper)
    {
    }
}