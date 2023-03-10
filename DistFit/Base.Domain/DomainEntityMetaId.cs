using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;

namespace Base.Domain;

public abstract class DomainEntityMetaId : DomainEntityMetaId<Guid>, IDomainEntityId
{
    
}

public abstract class DomainEntityMetaId<TKey> : DomainEntityId<TKey>, IDomainEntityMeta
    where TKey : IEquatable<TKey>
{
    [MaxLength(32)]
    [Display(ResourceType = typeof(Resources.Common), Name = nameof(CreatedBy))]
    public string? CreatedBy { get; set; }
    [Display(ResourceType = typeof(Resources.Common), Name = nameof(CreatedAt))]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [MaxLength(32)]
    [Display(ResourceType = typeof(Resources.Common), Name = nameof(UpdatedBy))]
    public string? UpdatedBy { get; set; }
    [Display(ResourceType = typeof(Resources.Common), Name = nameof(UpdatedAt))]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    [MaxLength(1024)]
    [Display(ResourceType = typeof(Resources.Common), Name = nameof(Comment))]
    public string? Comment { get; set; }
}