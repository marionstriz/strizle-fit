using Base.Contracts.Domain;
using Microsoft.AspNetCore.Identity;

namespace Base.Domain.Identity;

public abstract class BaseUser : BaseUser<Guid>, IDomainEntityId
{
}

public abstract class BaseUser<TKey> : IdentityUser<TKey>, IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
{
}