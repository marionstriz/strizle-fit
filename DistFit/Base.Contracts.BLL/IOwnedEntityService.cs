using Base.Contracts.DAL;
using Base.Contracts.Domain;

namespace Base.Contracts.BLL;

public interface IOwnedEntityService<TEntity> : IOwnedEntityRepository<TEntity>, IOwnedEntityService<TEntity, Guid> 
    where TEntity : class, IDomainEntityId
{

}

public interface IOwnedEntityService<TEntity, TKey> : IOwnedEntityRepository<TEntity, TKey>
    where TEntity : class, IDomainEntityId<TKey> 
    where TKey : IEquatable<TKey>
{

}