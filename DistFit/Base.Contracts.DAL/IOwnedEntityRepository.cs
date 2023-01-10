using Base.Contracts.Domain;

namespace Base.Contracts.DAL;

public interface IOwnedEntityRepository<TEntity> : IOwnedEntityRepository<TEntity, Guid> 
    where TEntity : class, IDomainEntityId
{
}

public interface IOwnedEntityRepository<TEntity, TKey> : IEntityRepository<TEntity, TKey> 
    where TEntity : class, IDomainEntityId<TKey> 
    where TKey : IEquatable<TKey>
{
    Task<IEnumerable<TEntity>> GetAllAsync(TKey userId, bool noTracking);
}