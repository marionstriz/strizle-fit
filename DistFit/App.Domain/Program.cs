using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.Domain;

public class Program : DomainEntityMetaId
{
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(Base.Resources.Common), Name = nameof(Name))]
    public LangStr Name { get; set; } = default!;
    [Column(TypeName = "jsonb")]
    [Display(ResourceType = typeof(Base.Resources.Common), Name = nameof(Description))]
    public LangStr? Description { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(IsPublic))]
    public bool IsPublic { get; set; }
    [Precision(12, 3)]
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(Duration))]
    public decimal? Duration { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(DurationUnitId))]
    public Guid? DurationUnitId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(DurationUnit))]
    public Unit? DurationUnit { get; set; }
    
    public ICollection<ProgramSaved>? ProgramSaves { get; set; }
    public ICollection<Session>? Sessions { get; set; }
}