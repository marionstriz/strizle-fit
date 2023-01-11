using App.Contracts.DAL;
using Base.Contracts.Base;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class AppUserRepository : 
    BaseEntityRepository<DTO.Identity.AppUser, Domain.Identity.AppUser, AppDbContext>, IAppUserRepository
{
    public AppUserRepository(
        AppDbContext dbContext, 
        IMapper<DTO.Identity.AppUser, Domain.Identity.AppUser> mapper) 
        : base(dbContext, mapper)
    {
    }
}