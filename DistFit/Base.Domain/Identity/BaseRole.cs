using Base.Contracts.Domain;
using Microsoft.AspNetCore.Identity;

namespace Base.Domain.Identity;

public abstract class BaseRole : BaseRole<Guid>, IDomainEntityId
{
}

public abstract class BaseRole<TKey> : IdentityRole<TKey>, IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
{
    protected BaseRole()
    {
    }

    protected BaseRole(string roleName): base(roleName)
    {
    }
}