namespace Base.Contracts.DAL;

public interface IUnitOfWork
{
    int SaveChanges();
    Task<int> SaveChangesAsync();
}