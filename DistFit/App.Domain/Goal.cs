using App.Domain.Identity;
using Base.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.Domain;

public class Goal : DomainEntityMetaId
{
    [Precision(24, 3)]
    public decimal Value { get; set; }
    public Guid ValueUnitId { get; set; }
    public Unit? ValueUnit { get; set; }
    public DateTime? ReachedAt { get; set; }
    
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    
    public Guid? MeasurementTypeId { get; set; }
    public MeasurementType? MeasurementType { get; set; }
    
    public Guid? ExerciseTypeId { get; set; }
    public ExerciseType? ExerciseType { get; set; }
}