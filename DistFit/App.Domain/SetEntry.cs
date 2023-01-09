using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.Domain;

public class SetEntry : DomainEntityMetaId
{
    [Precision(12, 3)]
    public decimal Quantity { get; set; }
    public Guid QuantityUnitId { get; set; }
    public Unit? QuantityUnit { get; set; }
    
    [Precision(12, 3)]
    public decimal? Weight { get; set; }
    public Guid? WeightUnitId { get; set; }
    public Unit? WeightUnit { get; set; }
    
    public Guid PerformanceId { get; set; }
    public Performance? Performance { get; set; }

    [MaxLength(1024)]
    public string? FilePath { get; set; }
}