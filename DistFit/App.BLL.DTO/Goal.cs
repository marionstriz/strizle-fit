using Base.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.BLL.DTO;

public class Goal : DomainEntityId
{
    [Precision(24, 3)]
    public decimal Value { get; set; }
    public Guid ValueUnitId { get; set; }
    public App.BLL.DTO.Unit? ValueUnit { get; set; }
    public DateTime? ReachedAt { get; set; }
    
    public Guid AppUserId { get; set; }
    public App.BLL.DTO.Identity.AppUser? AppUser { get; set; }
    
    public Guid? MeasurementTypeId { get; set; }
    public App.BLL.DTO.MeasurementType? MeasurementType { get; set; }
    
    public Guid? ExerciseTypeId { get; set; }
    public App.BLL.DTO.ExerciseType? ExerciseType { get; set; }
}