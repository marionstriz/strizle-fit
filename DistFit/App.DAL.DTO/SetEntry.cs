using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.DTO;

public class SetEntry : DomainEntityId
{
    [Precision(12, 3)]
    public decimal Quantity { get; set; }
    public Guid QuantityUnitId { get; set; }
    public App.DAL.DTO.Unit? QuantityUnit { get; set; }
    
    [Precision(12, 3)]
    public decimal? Weight { get; set; }
    public Guid? WeightUnitId { get; set; }
    public App.DAL.DTO.Unit? WeightUnit { get; set; }
    
    public Guid PerformanceId { get; set; }
    public DAL.DTO.Performance? Performance { get; set; }
}