using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.Domain;

public class Measurement : DomainEntityMetaId
{
    [Precision(24, 3)]
    [Display(ResourceType = typeof(Base.Resources.Common), Name = nameof(Value))]
    public decimal Value { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(ValueUnitId))]
    public Guid ValueUnitId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(ValueUnit))]
    public Unit? ValueUnit { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(MeasurementTypeId))]
    public Guid MeasurementTypeId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(MeasurementType))]
    public MeasurementType? MeasurementType { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(MeasuredAt))]
    public DateTime MeasuredAt { get; set; }
    
    [Display(ResourceType = typeof(Base.Resources.Common), Name = nameof(AppUserId))]
    public Guid AppUserId { get; set; }
    [Display(ResourceType = typeof(Base.Resources.Common), Name = nameof(AppUser))]
    public AppUser? AppUser { get; set; }
}