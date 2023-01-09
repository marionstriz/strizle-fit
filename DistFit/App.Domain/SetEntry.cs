using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.Domain;

public class SetEntry : DomainEntityMetaId
{
    [Precision(12, 3)]
    [Display(ResourceType = typeof(Base.Resources.Common), Name = nameof(Quantity))]
    public decimal Quantity { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(QuantityUnitId))]
    public Guid QuantityUnitId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(QuantityUnit))]
    public Unit? QuantityUnit { get; set; }
    
    [Precision(12, 3)]
    [Display(ResourceType = typeof(Base.Resources.Common), Name = nameof(Weight))]
    public decimal? Weight { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(WeightUnitId))]
    public Guid? WeightUnitId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(WeightUnit))]
    public Unit? WeightUnit { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(PerformanceId))]
    public Guid PerformanceId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(Performance))]
    public Performance? Performance { get; set; }
}