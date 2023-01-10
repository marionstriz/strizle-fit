using Base.Contracts.DAL;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF;

public class BaseUow<TDbContext> : IUnitOfWork 
    where TDbContext : DbContext
{
    protected readonly TDbContext UowDbContext;
    protected BaseUow(TDbContext dbContext)
    {
        UowDbContext = dbContext;
    }
    public virtual int SaveChanges()
    {
        return UowDbContext.SaveChanges();
    }

    public virtual async Task<int> SaveChangesAsync()
    {
        return await UowDbContext.SaveChangesAsync();
    }
}