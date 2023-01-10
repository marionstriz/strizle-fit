using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace Base.BLL;

public abstract class BaseBll<TDal> : IBll 
    where TDal : IUnitOfWork
{
    public abstract int SaveChanges();
    public abstract Task<int> SaveChangesAsync();
}