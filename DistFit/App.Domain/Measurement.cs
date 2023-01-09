using App.Domain.Identity;
using Base.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.Domain;

public class Measurement : DomainEntityMetaId
{
    [Precision(24, 3)]
    public decimal Value { get; set; }
    public Guid ValueUnitId { get; set; }
    public Unit? ValueUnit { get; set; }
    
    public Guid MeasurementTypeId { get; set; }
    public MeasurementType? MeasurementType { get; set; }
    
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}