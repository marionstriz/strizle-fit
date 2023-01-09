using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;

namespace Base.Domain;

public abstract class DomainEntityId : DomainEntityId<Guid>, IDomainEntityId
{
}

public abstract class DomainEntityId<TKey> : IDomainEntityId<TKey> 
    where TKey : IEquatable<TKey>
{
    //[Display(ResourceType = typeof(Resources.Common), Name = nameof(Id))]
    public TKey Id { get; set; } = default!;
}