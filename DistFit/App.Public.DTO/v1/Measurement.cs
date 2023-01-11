using System.ComponentModel.DataAnnotations;
using App.Public.DTO.v1.Identity;
using Base.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.Public.DTO.v1;

public class Measurement : DomainEntityId
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
    public DateTime MeasuredAt { get; set; } = DateTime.UtcNow;
    
    [Display(ResourceType = typeof(Base.Resources.Common), Name = nameof(AppUserId))]
    public Guid AppUserId { get; set; }
    
    [Display(ResourceType = typeof(Base.Resources.Common), Name = nameof(AppUser))]
    public AppUser? AppUser { get; set; }
}