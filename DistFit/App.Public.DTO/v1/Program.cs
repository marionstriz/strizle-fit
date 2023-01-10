using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.Public.DTO.v1;

public class Program : DomainEntityId
{
    [Display(ResourceType = typeof(Base.Resources.Common), Name = nameof(Name))]
    public string Name { get; set; } = default!;
    [Display(ResourceType = typeof(Base.Resources.Common), Name = nameof(Description))]
    public string? Description { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(IsPublic))]
    public bool IsPublic { get; set; }
    [Precision(12, 3)]
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(Duration))]
    public decimal? Duration { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(DurationUnitId))]
    public Guid? DurationUnitId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(DurationUnit))]
    public Unit? DurationUnit { get; set; }
}